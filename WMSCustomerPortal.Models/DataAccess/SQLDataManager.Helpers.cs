using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using System.Globalization;

using Microsoft.Practices.EnterpriseLibrary.Data;

namespace WMSCustomerPortal.Models.DataAccess {

    #region Extensions

    public static class DatabaseExtensions {
        public static IEnumerable<TResult> ExecuteNamedSprocAccessor<TResult>(this Database database, string procedureName, NamedParameterMapper parameterMapper, params object[] parameterValues)
            where TResult : new() {
            using (DbCommand cmd = database.GetStoredProcCommand(procedureName)) {
                database.DiscoverParameters(cmd);
                return new CustomSprocAccessor<TResult>(database, cmd, parameterMapper).Execute(parameterValues);
            }
        }
    }
    #endregion    

    #region Accessors

    public class CustomSprocAccessor<TResult> : SprocAccessor<TResult> where TResult : new() {
        private DbCommand command;
        IParameterMapper parameterMapper;

        public CustomSprocAccessor(Database db, DbCommand command, IParameterMapper parameterMapper)
            : base(db, command.CommandText, parameterMapper, MapBuilder<TResult>.BuildAllProperties()) {
            this.command = command;
            this.parameterMapper = parameterMapper;
        }

        public override IEnumerable<TResult> Execute(params object[] parameterValues) {
            parameterMapper.AssignParameters(command, parameterValues);
            return base.Execute(command);
        }
    }
    #endregion

    #region Parameter Mappers

    public class DefaultParameterMapper : IParameterMapper {
        readonly Database database;
        public DefaultParameterMapper(Database database) {
            this.database = database;
        }
        public void AssignParameters(DbCommand command, object[] parameterValues) {
            if (parameterValues.Length > 0) {
                GuardParameterDiscoverySupported();
                database.AssignParameters(command, parameterValues);
            }
        }

        private void GuardParameterDiscoverySupported() {
            if (!database.SupportsParemeterDiscovery) {
                throw new InvalidOperationException(
                    string.Format(CultureInfo.CurrentCulture,
                                    "Resources.ExceptionParameterDiscoveryNotSupported",
                                    database.GetType().FullName));
            }
        }
    }

    public class NamedParameterMapper : IParameterMapper {
        private string[] parametersToUse;

        public NamedParameterMapper(params string[] parametersToUse) {
            this.parametersToUse = parametersToUse;
        }

        public void AssignParameters(DbCommand command, object[] parameterValues) {
            int parameterValueIndex = 0;

            for (int i = 0; i < command.Parameters.Count; i++) {
                if (parametersToUse.Contains(command.Parameters[i].ParameterName, StringComparer.OrdinalIgnoreCase) &&
                    command.Parameters[i].Direction == System.Data.ParameterDirection.Input) {
                    command.Parameters[i].Value = parameterValues[parameterValueIndex++];
                }
            }
        }
    }
    #endregion

    public class Helpers {        

        public List<T> BindDataList<T>(DataTable dt) {
            List<string> columns = new List<string>();
            foreach (DataColumn dc in dt.Columns) {
                columns.Add(dc.ColumnName);
            }

            var fields = typeof(T).GetFields();
            var properties = typeof(T).GetProperties();

            List<T> lst = new List<T>();

            foreach (DataRow dr in dt.Rows) {
                var ob = Activator.CreateInstance<T>();

                foreach (var fieldInfo in fields) {
                    if (columns.Contains(fieldInfo.Name)) {
                        fieldInfo.SetValue(ob, dr[fieldInfo.Name]);
                    }
                }

                foreach (var propertyInfo in properties) {
                    if (columns.Contains(propertyInfo.Name)) {
                        propertyInfo.SetValue(ob, dr[propertyInfo.Name]);
                    }
                }
                lst.Add(ob);
            }
            return lst;
        }
    }
}
