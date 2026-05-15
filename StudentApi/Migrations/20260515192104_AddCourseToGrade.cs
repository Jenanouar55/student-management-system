using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentApi.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseToGrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Subject column was already dropped by a previous partial run — skip it

            // Create Courses table first
            migrationBuilder.Sql("CREATE TABLE IF NOT EXISTS `Courses` (`Id` int NOT NULL AUTO_INCREMENT, `Name` longtext NOT NULL, `Description` longtext NOT NULL, `Credits` int NOT NULL, `Teacher` longtext NOT NULL, PRIMARY KEY (`Id`)) CHARACTER SET utf8mb4;");

            // Insert a default course so existing Grades rows can reference it
            migrationBuilder.Sql("INSERT INTO `Courses` (`Name`, `Description`, `Credits`, `Teacher`) SELECT 'Cours par défaut', 'Cours assigné automatiquement à la migration', 0, '' WHERE NOT EXISTS (SELECT 1 FROM `Courses`);");

            // Add CourseId nullable first
            migrationBuilder.Sql("ALTER TABLE `Grades` ADD COLUMN IF NOT EXISTS `CourseId` int NULL;");

            // Point all existing grades to the default course
            migrationBuilder.Sql("UPDATE `Grades` SET `CourseId` = (SELECT MIN(`Id`) FROM `Courses`) WHERE `CourseId` IS NULL;");

            // Make it non-nullable
            migrationBuilder.Sql("ALTER TABLE `Grades` MODIFY `CourseId` int NOT NULL DEFAULT 0;");

            // Add index if not exists
            migrationBuilder.Sql("CREATE INDEX IF NOT EXISTS `IX_Grades_CourseId` ON `Grades` (`CourseId`);");

            // Add foreign key if not exists
            migrationBuilder.Sql(@"
                SET @fk_exists = (
                    SELECT COUNT(*) FROM information_schema.TABLE_CONSTRAINTS
                    WHERE CONSTRAINT_SCHEMA = DATABASE()
                    AND TABLE_NAME = 'Grades'
                    AND CONSTRAINT_NAME = 'FK_Grades_Courses_CourseId'
                    AND CONSTRAINT_TYPE = 'FOREIGN KEY'
                );
                SET @sql = IF(@fk_exists = 0,
                    'ALTER TABLE `Grades` ADD CONSTRAINT `FK_Grades_Courses_CourseId` FOREIGN KEY (`CourseId`) REFERENCES `Courses` (`Id`) ON DELETE CASCADE',
                    'SELECT 1'
                );
                PREPARE stmt FROM @sql;
                EXECUTE stmt;
                DEALLOCATE PREPARE stmt;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grades_Courses_CourseId",
                table: "Grades");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Grades_CourseId",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Grades");

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Grades",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
