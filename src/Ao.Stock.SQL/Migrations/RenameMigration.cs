using FluentMigrator;

namespace Ao.Stock.SQL.Migrations
{
    public class RenameMigration : Migration
    {
        public RenameMigration(IEnumerable<RenameInput> renameInputs)
        {
            RenameInputs = renameInputs ?? throw new ArgumentNullException(nameof(renameInputs));
        }

        public IEnumerable<RenameInput> RenameInputs { get; }

        public override void Down()
        {
            foreach (var item in RenameInputs)
            {
                Rename.Column(item.NewName)
                    .OnTable(item.Table)
                    .To(item.OldName);
            }
        }

        public override void Up()
        {
            foreach (var item in RenameInputs)
            {
                Rename.Column(item.OldName)
                    .OnTable(item.Table)
                    .To(item.NewName);
            }
        }
    }
}
