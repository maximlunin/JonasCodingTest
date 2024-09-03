using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DataAccessLayer.Model.Interfaces;
using DataAccessLayer.Model.Models;
using Serilog;

namespace DataAccessLayer.Database
{
    public class InMemoryEmployeeDatabase<T> : IDbWrapper<T> where T : EmployeeDataEntity
    {
        private static Dictionary<Tuple<string, string>, EmployeeDataEntity> DatabaseInstance;
        private readonly ILogger _logger;

        static InMemoryEmployeeDatabase()
        {
            DatabaseInstance = new Dictionary<Tuple<string, string>, EmployeeDataEntity>();
            InitializingTestData();
        }

        public InMemoryEmployeeDatabase(ILogger logger)
        {
            _logger = logger;
        }

        public static void InitializingTestData()
        {
            DatabaseInstance.Add(Tuple.Create("Company1", "Employee1"), new Employee()
            {
                CompanyCode = "Company1",
                SiteId = "Site1",
                EmployeeCode = "Employee1",
                EmailAddress = "abc@.din.com",
                EmployeeStatus = "active",
                Phone = "312-345-3344",
                Occupation = "Doctor",
                EmployeeName = "John Doe"
            });

            DatabaseInstance.Add(Tuple.Create("Company1", "Employee2"), new Employee()
            {
                CompanyCode = "Company2",
                SiteId = "Site1",
                EmployeeCode = "Employee2",
                EmailAddress = "abc@.din.com",
                EmployeeStatus = "active",
                Phone = "312-345-3344",
                Occupation = "Doctor",
                EmployeeName = "Frint Doe"
            });

            DatabaseInstance.Add(Tuple.Create("Company2", "Employee3"), new Employee()
            {
                CompanyCode = "Company2",
                SiteId = "Site2",
                EmployeeCode = "Employee3",
                EmailAddress = "abc@.din.com",
                EmployeeStatus = "active",
                Phone = "312-345-3344",
                Occupation = "Doctor",
                EmployeeName = "fdsd Doe"
            });

            DatabaseInstance.Add(Tuple.Create("Company2", "Employee4"), new Employee()
            {
                CompanyCode = "Company2",
                SiteId = "Site2",
                EmployeeCode = "Employee4",
                EmailAddress = "abc@.din.com",
                EmployeeStatus = "active",
                Phone = "312-345-3344",
                Occupation = "Doctor",
                EmployeeName = "Fridfggfnt Doe"
            });

        }

        public bool Insert(T data)
        {
            try
            {
                DatabaseInstance.Add(Tuple.Create(data.CompanyCode, data.EmployeeCode), data);
                return true;
            }
            catch(Exception ex)
            {
                _logger.Error("Error in InMemoryEmployeeDatabase(Method : Insert) :" + " { " + " companyCode : " + data.CompanyCode + " , employeeCode : " + data.EmployeeCode + " }"
                    + "\nException: " + ex.Message);
                throw ex;
            }
        }

        public bool Update(T data)
        {
            try
            {
                if (DatabaseInstance.ContainsKey(Tuple.Create(data.CompanyCode, data.EmployeeCode)))
                {
                    DatabaseInstance.Remove(Tuple.Create(data.CompanyCode, data.EmployeeCode));
                    Insert(data);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.Error("Error in InMemoryEmployeeDatabase(Method : Update) :" + " { " + " companyCode : " + data.CompanyCode + " , employeeCode : " + data.EmployeeCode + " }"
                    + "\nException: " + ex.Message);
                return false;
            }
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
        {
            try
            {
                var entities = FindAll();
                return entities.Where(expression.Compile());
            }
            catch (Exception ex)
            {
                _logger.Error("Error in InMemoryEmployeeDatabase(Method : Find) :" + expression.Body +"\nException: " + ex.Message);
                return Enumerable.Empty<T>();
            }
        }

        public IEnumerable<T> FindAll()
        {
            try
            {
                return DatabaseInstance.Values.OfType<T>();
            }
            catch (Exception ex)
            {
                _logger.Error("Error in InMemoryEmployeeDatabase(Method : FindAll) :\nException: " + ex.Message);
                throw ex;              
            }
        }

        public bool Delete(Expression<Func<T, bool>> expression)
        {
            try
            {
                var entities = FindAll();
                var entity = entities.Where(expression.Compile());
                foreach (var dataEntity in entity.ToList())
                {
                    DatabaseInstance.Remove(Tuple.Create(dataEntity.CompanyCode, dataEntity.EmployeeCode));
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Error in InMemoryEmployeeDatabase(Method : Delete) :\nException: " + ex.Message);
                throw ex;
            }
        }

        public bool DeleteAll()
        {
            try
            {
                DatabaseInstance.Clear();
                return true;
            }
            catch (Exception ex)
            {
                _logger.Error("Error in InMemoryEmployeeDatabase(Method : DeleteAll) :\nException: " + ex.Message);
                return false;
            }
        }

        public bool UpdateAll(Expression<Func<T, bool>> filter, string fieldToUpdate, object newValue)
        {
            try
            {
                var entities = FindAll();
                var entity = entities.Where(filter.Compile());
                foreach (var dataEntity in entity.ToList())
                {
                    var newEntity = UpdateProperty(dataEntity, fieldToUpdate, newValue);

                    DatabaseInstance.Remove(Tuple.Create(dataEntity.CompanyCode, dataEntity.EmployeeCode));
                    Insert(newEntity);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private T UpdateProperty(T dataEntity, string key, object value)
        {
            Type t = typeof(T);
            var prop = t.GetProperty(key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (prop == null)
            {
                throw new Exception("Property not found");
            }

            prop.SetValue(dataEntity, value, null);
            return dataEntity;
        }

        public Task<bool> InsertAsync(T data)
        {
            return Task.FromResult(Insert(data));
        }

        public Task<bool> UpdateAsync(T data)
        {
            return Task.FromResult(Update(data));
        }

        public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> expression)
        {
            return Task.FromResult(Find(expression));
        }

        public Task<IEnumerable<T>> FindAllAsync()
        {
            return Task.FromResult(FindAll());
        }

        public Task<bool> DeleteAsync(Expression<Func<T, bool>> expression)
        {
            return Task.FromResult(Delete(expression));
        }

        public Task<bool> DeleteAllAsync()
        {
            return Task.FromResult(DeleteAll());
        }

        public Task<bool> UpdateAllAsync(Expression<Func<T, bool>> filter, string fieldToUpdate, object newValue)
        {
            return Task.FromResult(UpdateAll(filter, fieldToUpdate, newValue));
        }


    }
}


