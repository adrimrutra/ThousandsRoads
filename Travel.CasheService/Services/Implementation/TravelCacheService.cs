using System;
using System.Collections.Generic;
using System.Linq;
using Travel.Service.Services.Abstractions;

using InterSystems.Data.CacheTypes;
using ThousandsRoads;
using Travel.CasheMigration;
using Travel.CasheService.Services.Abstraction;
using Travel.Common.Enums;

namespace Travel.CasheService.Services.Implementation
{
	public class TravelCacheService : BaseCacheService<ThousandsRoads.Travel, Data.Travel>, ITravelService
	{
		public TravelCacheService(CacheSession<ThousandsRoads.Travel, Data.Travel> session, CacheSession<ThousandsRoads.Traveler, Data.Traveler> pasangerSession)
			: base(session)
		{
			_passangerSession = pasangerSession;
		}
		private CacheSession<ThousandsRoads.Traveler, Data.Traveler> _passangerSession;
		public override Data.Travel Add(Data.Travel model)
		{
			using (var session = this.cacheSession.Open())
			{
				var travel = new ThousandsRoads.Travel(session);
				SaveStrategy(model, travel);
				var result = this.cacheSession.Add(travel);
				return result;
			}
		}
		public Data.Traveler AddPassanger(Data.Traveler model)
		{
			using (var session = this._passangerSession.Open())
			{
				var traveler = new ThousandsRoads.Traveler(session);
				traveler.CustomerType = (long)model.Usertype;
				traveler.User = ThousandsRoads.Customer.OpenId(traveler.Connection, model.UserId.ToString());
				traveler.Travel = ThousandsRoads.Travel.OpenId(traveler.Connection, model.TravelId.ToString());
				var result = this._passangerSession.Add(traveler);
				return result;
			}
		}
		public void DeletePassanger(int id)
		{
			using (var session = this._passangerSession.Open())
			{
				this._passangerSession.Delete(id);
			}
		}
		private void SaveStrategy(Data.Travel model, ThousandsRoads.Travel travel)
		{
			travel.Capacity = model.Capacity;
			travel.Startdate = model.Startdate;
			//travel.Starttime.Value.AddHours(model.Startdate.Hour);
			//travel.Starttime.Value.AddMinutes(model.Startdate.Minute);
			//travel.Starttime = model.Startdate.ToUniversalTime();
			travel.Enddate = model.Enddate;
			travel.Mapsnapshot = model.Mapsnapshot;
			travel.Luggage = (long)model.Luggage;
			travel.Travelers.Clear();
			foreach (var tr in model.Travelers.Where(x => x.UserId != 0).ToList())
			{
				Customer customer = Customer.OpenId(travel.Connection, tr.UserId.ToString());
				if (customer != null)
				{
					if (travel.Travelers == null)
						travel.Travelers = new CacheListOfObjects();
					var fashist = new ThousandsRoads.Traveler(travel.Connection);
					fashist.CustomerType = (long)tr.Usertype;
					fashist.User = customer;
					fashist.Travel = travel;
					travel.DriverId = long.Parse(customer.Id());
					travel.Travelers.Add(fashist);
				}
			}

			travel.MapPoints.Clear();
			foreach (var point in model.MapPoints)
			{
				travel.MapPoints.Add(new ThousandsRoads.MapPoint(travel.Connection)
				{
					Latitude = point.Latitude,
					Longitude = point.Longitude,
					Name = point.Name,
				});
			}
			var firstpoint = model.MapPoints.FirstOrDefault();
			travel.Startpoint = firstpoint == null ? "" : firstpoint.Name;
			var endpoint = model.MapPoints.LastOrDefault();
			travel.Endpoint = endpoint == null ? "" : endpoint.Name;
		}
		public override Data.Travel Save(Data.Travel model)
		{
			using (var session = this.cacheSession.Open())
			{
				var driver = model.Travelers.SingleOrDefault(x => x.Usertype == UserType.Driver);
				if (driver != null && driver.UserId != 0)
				{
					Customer tmp = Customer.OpenId(session, driver.UserId.ToString());
					if (tmp != null)
					{
						var entity = this.cacheSession.GetReal(model.Id);
						SaveStrategy(model, entity);
						entity.Save();
						return this.cacheSession.Mapper.Map<ThousandsRoads.Travel, Data.Travel>(entity);
					}
				}
			}
			throw new ArgumentException("something wrong");
		}
		public override IEnumerable<Data.Travel> GetAll()
		{
			using (var session = this.cacheSession.Open())
			{
				this.cacheSession.SelectCommand.CommandText = @"select
																ThousandsRoads.Travel.ID , Capacity, DriverId, Enddate, Endpoint,
																Startdate, Startpoint ,Mapsnapshot ,DriverId, DisplayName, Rating 
																from ThousandsRoads.Travel 
																left join ThousandsRoads.Customer
																on DriverId = ThousandsRoads.Customer.ID
																where DriverId is not null
																order by Startdate desc";
				return this.cacheSession.GetAll();//.Map<Tsource, Tdest>(mapper.Map<Tsource, Tdest>);
			}
		}
		public bool PutTraveler(int DriverId, int TravelId, int UserId)
		{
			using (var session = this.cacheSession.Open())
			{
				ThousandsRoads.Travel travel = ThousandsRoads.Travel.OpenId(session, TravelId.ToString());
				ThousandsRoads.Customer user = Customer.OpenId(session, UserId.ToString());
				if ((travel != null && travel.Id() != "0") && (user != null && user.Id() != "0"))
				{
					if (travel.DriverId == DriverId)
					{
						ThousandsRoads.Traveler traveler = new Traveler(session);
						traveler.User = user;
						traveler.CustomerType = 2;
						traveler.Travel = travel;
						travel.Travelers.Add(traveler);
						travel.Save();
						traveler.Save();
						return true;
					}
				}
			}
			return false;
		}


		public IEnumerable<Data.Travel> GetTravelsByIds(IEnumerable<int> Ids)
		{
			using (var session = this.cacheSession.Open())
			{
				if (Ids.Count() != 0)
				{
					var query = @"select 
									ThousandsRoads.Travel.ID , Capacity, DriverId, Enddate, Endpoint,
									Startdate, Startpoint ,Mapsnapshot ,DriverId, DisplayName, Rating 
									from ThousandsRoads.Travel 
									left join ThousandsRoads.Customer
									on DriverId = ThousandsRoads.Customer.ID
									where DriverId is not null
									and ThousandsRoads.Travel.ID = ";
					var where = string.Join("or ThousandsRoads.Travel.ID = ", Ids);
					this.cacheSession.SelectCommand.CommandText = query + where;

					var result = this.cacheSession.GetAll();//.Map<Tsource, Tdest>(mapper.Map<Tsource, Tdest>);
					return result;
				}
				else
					return new List<Data.Travel>();
			}
		}
	}
}
