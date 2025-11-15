using System;
using Microsoft.EntityFrameworkCore.Migrations;

public partial class FixIdentityKeys : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        // Este SQL realiza, en orden seguro:
        // 1) elimina default constraint sobre AspNetRoles.Id (si existe)
        // 2) elimina FKs que referencian AspNetRoles
        // 3) elimina PK de AspNetRoles
        // 4) crea columnas temporales GUID, convierte valores con TRY_CONVERT
        // 5) sustituye columnas antiguas por las nuevas (renombrado)
        // 6) recrea PK y FK
        migrationBuilder.Sql(@"
BEGIN TRANSACTION;

-- 1) Drop default constraint on AspNetRoles.Id (if any)
DECLARE @df sysname;
SELECT @df = d.name
FROM sys.default_constraints d
JOIN sys.columns c ON d.parent_object_id = c.object_id AND d.parent_column_id = c.column_id
WHERE d.parent_object_id = OBJECT_ID(N'[dbo].[AspNetRoles]') AND c.name = N'Id';
IF @df IS NOT NULL
    EXEC('ALTER TABLE [dbo].[AspNetRoles] DROP CONSTRAINT [' + @df + ']');

-- 2) Drop foreign keys that reference AspNetRoles
DECLARE @fkName sysname, @parentSchema sysname, @parentTable sysname, @sql nvarchar(max);
DECLARE fk_cursor CURSOR FOR
SELECT fk.name, SCHEMA_NAME(tp.schema_id), tp.name
FROM sys.foreign_keys fk
JOIN sys.tables tr ON fk.referenced_object_id = tr.object_id
JOIN sys.tables tp ON fk.parent_object_id = tp.object_id
WHERE tr.object_id = OBJECT_ID(N'[dbo].[AspNetRoles]');
OPEN fk_cursor;
FETCH NEXT FROM fk_cursor INTO @fkName, @parentSchema, @parentTable;
WHILE @@FETCH_STATUS = 0
BEGIN
    SET @sql = N'ALTER TABLE [' + @parentSchema + N'].[' + @parentTable + N'] DROP CONSTRAINT [' + @fkName + N']';
    EXEC sp_executesql @sql;
    FETCH NEXT FROM fk_cursor INTO @fkName, @parentSchema, @parentTable;
END
CLOSE fk_cursor;
DEALLOCATE fk_cursor;

-- 3) Drop primary key on AspNetRoles (if exists)
IF EXISTS (SELECT 1 FROM sys.key_constraints kc WHERE kc.parent_object_id = OBJECT_ID(N'[dbo].[AspNetRoles]') AND kc.name = N'PK_AspNetRoles')
BEGIN
    ALTER TABLE [dbo].[AspNetRoles] DROP CONSTRAINT [PK_AspNetRoles];
END

-- 4) Add temporary GUID column and populate it converting existing string ids when possible
ALTER TABLE [dbo].[AspNetRoles] ADD [Id_temp] UNIQUEIDENTIFIER NULL;
UPDATE ar SET Id_temp = TRY_CONVERT(uniqueidentifier, ar.Id) FROM [dbo].[AspNetRoles] ar;
-- If some Ids are not valid GUID strings, assign new GUIDs — revisa estos casos manualmente después si es crítico
UPDATE [dbo].[AspNetRoles] SET Id_temp = NEWID() WHERE Id_temp IS NULL;

-- 5) Build mapping table OldId -> NewId
IF OBJECT_ID('tempdb..#RoleIdMap') IS NOT NULL DROP TABLE #RoleIdMap;
CREATE TABLE #RoleIdMap (OldId NVARCHAR(450) PRIMARY KEY, NewId UNIQUEIDENTIFIER);
INSERT INTO #RoleIdMap (OldId, NewId)
SELECT Id, Id_temp FROM [dbo].[AspNetRoles];

-- 6) Convert AspNetUserRoles.RoleId (and any other tables referencing role id)
-- Add temp column
IF COL_LENGTH('dbo.AspNetUserRoles','RoleId_temp') IS NULL
    ALTER TABLE [dbo].[AspNetUserRoles] ADD [RoleId_temp] UNIQUEIDENTIFIER NULL;

-- Populate RoleId_temp using mapping
UPDATE aur
SET RoleId_temp = m.NewId
FROM [dbo].[AspNetUserRoles] aur
LEFT JOIN #RoleIdMap m ON aur.RoleId = m.OldId  ;

        // 3) Alter column Id to uniqueidentifier (ajusta oldClrType/oldType según tu esquema actual)
        migrationBuilder.AlterColumn<Guid>(
            name: "Id",
            table: "AspNetRoles",
            type: "uniqueidentifier",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(450)");

        // 4) Recreate primary key
        migrationBuilder.AddPrimaryKey(
            name: "PK_AspNetRoles",
            table: "AspNetRoles",
            column: "Id");

        // 5) Recreate foreign key(s)
        migrationBuilder.AddForeignKey(
            name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
            table: "AspNetUserRoles",
            column: "RoleId",
            principalTable: "AspNetRoles",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // Revert steps (ajusta tipos/nombres según original)
        migrationBuilder.DropForeignKey(
            name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
            table: "AspNetUserRoles");

        migrationBuilder.DropPrimaryKey(
            name: "PK_AspNetRoles",
            table: "AspNetRoles");

        migrationBuilder.AlterColumn<string>(
            name: "Id",
            table: "AspNetRoles",
            type: "nvarchar(450)",
            nullable: false,
            oldClrType: typeof(Guid),
            oldType: "uniqueidentifier");

        migrationBuilder.AddPrimaryKey(
            name: "PK_AspNetRoles",
            table: "AspNetRoles",
            column: "Id");

        migrationBuilder.AddForeignKey(
            name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
            table: "AspNetUserRoles",
            column: "RoleId",
            principalTable: "AspNetRoles",
            principalColumn: "Id",
            onDelete: ReferentialAction.Cascade);
    }
}