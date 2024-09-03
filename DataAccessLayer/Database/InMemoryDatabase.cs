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
	public class InMemoryDatabase<T> : IDbWrapper<T> where T : DataEntity
	{
		private static Dictionary<Tuple<string, string>, DataEntity> DatabaseInstance;
		private readonly ILogger _logger;

		static InMemoryDatabase()
		{
			DatabaseInstance = new Dictionary<Tuple<string, string>, DataEntity>();
			InitializingTestData();
		}

		public InMemoryDatabase(ILogger logger)
		{
			_logger = logger;
		}

		public static void InitializingTestData()
		{
			DatabaseInstance.Add(Tuple.Create("Site1", "Company1"), new Company()
			{
				CompanyCode= "Company1",
				CompanyName = "Sinrnt",
				SiteId = "Site1",
                AddressLine1 = "240 wellessley street",
				AddressLine2 ="Toronto",
                PostalZipCode = "M4X1G5",
				PhoneNumber ="5145263566",
				FaxNumber = "4165263566",
				EquipmentCompanyCode = "CC1",
				Country ="Canada"
			});

			DatabaseInstance.Add(Tuple.Create("Site2", "Company2"), new Company()
			{
				CompanyCode = "Company2",
				CompanyName = "Dirt",

				SiteId = "Site2",
				AddressLine1 = "240 wellessley street",
				AddressLine2 = "Toronto",
				PostalZipCode = "M4X1G5",
				PhoneNumber = "5145263566",
				FaxNumber = "4165263566",
				EquipmentCompanyCode = "CC1",
				Country = "Canada"
			}) ;

			DatabaseInstance.Add(Tuple.Create("Site2", "Company3"), new Company()
			{
				CompanyCode = "Company3",
				CompanyName = "ffre",

				SiteId = "Site2",
				AddressLine1 = "240 wellessley street",
				AddressLine2 = "Toronto",
				PostalZipCode = "M4X1G5",
				PhoneNumber = "5145263566",
				FaxNumber = "4165263566",
				EquipmentCompanyCode = "CC1",
				Country = "Canada"
			}) ;

            DatabaseInstance.Add(Tuple.Create("Site2", "Company4"), new Company()
            {
                CompanyCode = "Company4",
				CompanyName = "rfdgdfb",
                SiteId = "Site2",
                AddressLine1 = "240 wellessley street",
                AddressLine2 = "Toronto",
                PostalZipCode = "M4X1G5",
                PhoneNumber = "5145263566",
                FaxNumber = "4165263566",
                EquipmentCompanyCode = "CC1",
                Country = "Canada"
            });

        }

		public bool Insert(T data)
		{
			try
			{
				DatabaseInstance.Add(Tuple.Create(data.SiteId, data.CompanyCode), data);
				return true;
			}
            catch (Exception ex)
            {
                _logger.Error("InMemoryDatabase: Insert, CompanyCode :" +data?.CompanyCode + ", SitedID " + data?.SiteId + "\n Exception"  + ex.Message);
                throw ex;
            }
        }

		public bool Update(T data)
		{
			try
			{
				if (DatabaseInstance.ContainsKey(Tuple.Create(data.SiteId, data.CompanyCode)))
				{
					DatabaseInstance.Remove(Tuple.Create(data.SiteId, data.CompanyCode));
					Insert(data);
					return true;
				}

				return false;
			}
            catch (Exception ex)
            {
                _logger.Error("InMemoryDatabase: Update, Exception :" + ex.Message);
                throw ex;
            }
        }

		public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
		{
			try
			{
				var entities = FindAll();
				return entities.Where(expression.Compile());
			}
			catch(Exception ex)
			{
				_logger.Error("InMemoryDatabase: Find, Exception :" + ex.Message);
				throw ex; 
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
                _logger.Error("InMemoryDatabase: FindAll, Exception :" + ex.Message);
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
					DatabaseInstance.Remove(Tuple.Create(dataEntity.SiteId, dataEntity.CompanyCode));
				}
				
				return true;
			}
			catch (Exception ex)
            {
                _logger.Error("InMemoryDatabase: Delete, Exception :" + ex.Message);
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
                _logger.Error("InMemoryDatabase: DeleteAll, Exception :" + ex.Message);
                throw ex;
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

					DatabaseInstance.Remove(Tuple.Create(dataEntity.SiteId, dataEntity.CompanyCode));
					Insert(newEntity);
				}

				return true;
			}
			catch (Exception ex)
            {
                _logger.Error("InMemoryDatabase: UpdateAll, Exception :" + ex.Message);
                throw ex;
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
