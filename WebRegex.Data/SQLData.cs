using System;
using System.Collections.Generic;
using System.Text;
using WebRegex.Core.Models;
using Dapper;
using System.Data;
using System.Runtime.CompilerServices;
using System.Linq;

namespace WebRegex.Data
{
    public class SqlData
    {
        private readonly string ConnectionString;

        public SqlData(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<Profile> GetProfiles()
        {
            using IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
            return connection.Query<Profile>("select * from Profiles").ToList();
        }

        public List<Expression> GetExpressions(int profileId)
        {
            using IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
            return connection.Query<Expression>($"select * from Expressions where ProfileId = {profileId}").ToList();
        }

        public void SaveNewProfile(Profile profile, List<Expression> expressions)
        {
            using IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
            connection.Execute($"insert into dbo.Profiles values ('{profile.Name}')");
            var profileId = connection.Query<int>($"select Id from dbo.Profiles where Name = '{profile.Name}'");
            foreach (Expression expression in expressions)
            {
                connection.Execute($"insert into dbo.Expressions (ProfileId, Name, Regex) values ('{profileId.First()}', '{expression.Name}', '{expression.Regex}')");
            }
        }

        public void SaveExistingProfile(Profile profile, List<Expression> expressions)
        {
            using IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
            connection.Execute($"update dbo.Profiles set Name = {profile.Name} where Id = {profile.Id}");
            foreach (Expression expression in expressions)
            {
                if (expression.Id != 0)
                {
                    connection.Execute($"update dbo.Expressions set Name = {expression.Name} where Id = {expression.Id}");
                    connection.Execute($"update dbo.Expressions set Regex = {expression.Regex} where Id = {expression.Id}");
                }
                else
                {
                    connection.Execute(@$"insert into dbo.Expressions (ProfileId, Name, Regex) values ('{profile.Id}', '{expression.Name}', '{expression.Regex}')");
                }
            }
            foreach (Expression savedexpression in GetExpressions(profile.Id))
            {
                if(expressions.All(n => n.Name != savedexpression.Name))
                {
                    connection.Execute($"delete from dbo.expression where Id = {savedexpression.Id}");
                }
            }
        }

        public void DeleteProfile(Profile selectedProfile)
        {
            using IDbConnection connection = new System.Data.SqlClient.SqlConnection(ConnectionString);
            connection.Execute($"delete from dbo.Profiles where Id = {selectedProfile.Id}");
            connection.Execute($"delete from dbo.Expressions where ProfileId = {selectedProfile.Id}");
        }
    }
}
