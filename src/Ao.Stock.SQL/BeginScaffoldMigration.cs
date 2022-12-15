using Ao.Stock.Comparering;
using System;

namespace Ao.Stock.SQL
{
    public class BeginScaffoldMigration
    {
        private IStockType? oldType;
        private IReadOnlyList<IStockComparisonAction>? actions;

        public BeginScaffoldMigration(AutoMigrationHelper autoHelper, IStockType newType, IStockTypeComparer comparer)
        {
            AutoHelper = autoHelper;
            NewType = newType;
            Comparer = comparer;
        }

        public AutoMigrationHelper AutoHelper { get; }

        public IStockType NewType { get; }

        public IStockType? OldType { get; }

        public IStockTypeComparer Comparer { get; }

        public IReadOnlyList<IStockComparisonAction>? Actions => actions;

        public BeginScaffoldMigration Scaffold(string tableName)
        {
            return Scaffold(tableName, DefaultEFEntityTypeToStockConverter.Instance);
        }
        public BeginScaffoldMigration Scaffold(string tableName, IEFEntityTypeToStockConverter converter)
        {
            oldType = AutoHelper.ScaffoldHelper.OnlyTable(tableName).Scaffold().AsStockType(tableName, converter);
            return this;
        }

        public BeginScaffoldMigration Compare()
        {
            actions = Comparer.Compare(oldType, NewType);
            return this;
        }
        public BeginScaffoldMigration Migrate(Func<IReadOnlyList<IStockComparisonAction>, IReadOnlyList<IStockComparisonAction>>? project=null)
        {
            if (actions==null)
            {
                throw new InvalidOperationException("No actions to migrate");
            }
            var select = project?.Invoke(actions) ?? actions;
            AutoHelper.MigrationHelper.Migrate(select);
            return this;
        }
        public BeginScaffoldMigration ScaffoldCompareAndMigrate(
            string tableName, 
            IEFEntityTypeToStockConverter converter,
            Func<IReadOnlyList<IStockComparisonAction>, IReadOnlyList<IStockComparisonAction>>? project = null)
        {
            Scaffold(tableName,converter);
            Compare();
            return Migrate(project);
        }
        public BeginScaffoldMigration ScaffoldCompareAndMigrate(
            string tableName,
            Func<IReadOnlyList<IStockComparisonAction>, IReadOnlyList<IStockComparisonAction>>? project = null)
        {
            Scaffold(tableName);
            Compare();
            return Migrate(project);
        }
    }
}
