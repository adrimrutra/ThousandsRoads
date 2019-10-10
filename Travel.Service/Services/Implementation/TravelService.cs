using System.Collections.Generic;
using System.Linq;
using Travel.Core.Data;
using Travel.Service.Services.Abstractions;
using Travel.Service.Mapping;
using System.Data.Entity;
using Travel.Core;
using Travel.Common.Enums;

namespace Travel.Service.Services.Implementation
{
	public class TravelService : ITravelService
	{
		#region Dependency
		private IDbSession _dbSession { set; get; }
		private IObjectMapper _mapper { set; get; }
		public TravelService(IDbSession dbSession, IObjectMapper mapper)
		{
			this._dbSession = dbSession;
			this._mapper = mapper;
		}
		#endregion Dependency

		public Data.Travel Get(int id)
		{
			var entity = _dbSession.AsQueryable<Travel.Entity.Travel>()
				.Include(x => x.Travelers)
				.Include(x => x.Travelers.Select(m => m.User))
				.Include(x => x.MapPoints)
				.SingleOrDefault(x => x.Id == id);
			return _mapper.Map<Travel.Entity.Travel, Data.Travel>(entity);

		}

		public Data.Travel Add(Data.Travel model)
		{
			using (var trance = _dbSession.BeginTransaction())
			{
				var entity = new Entity.Travel();
				SaveStrategy(entity, model);
				_dbSession.Insert(entity);
				trance.Commit();
				return _mapper.Map<Travel.Entity.Travel, Data.Travel>(entity);
			}
		}

		public void Delete(int id)
		{
			using (var trance = _dbSession.BeginTransaction())
			{
				_dbSession.Delete<Travel.Entity.Travel>(x => x.Id == id);
				trance.Commit();
			}
		}

		public Data.Travel Save(Data.Travel model)
		{
			using (var trance = _dbSession.BeginTransaction())
			{
				var entity = _dbSession.AsQueryable<Travel.Entity.Travel>()
					.Include(x => x.Travelers)
					.Include(x => x.MapPoints)
					.SingleOrDefault(x => x.Id == model.Id);
				entity = SaveStrategy(entity, model);
				_dbSession.Save(entity);
				trance.Commit();
				return _mapper.Map<Travel.Entity.Travel, Data.Travel>(entity);
			}
		}

		private Entity.Travel SaveStrategy(Entity.Travel travel, Data.Travel model)
		{
			travel.Capacity = model.Capacity;
			travel.Startdate = model.Startdate;
			travel.Enddate = model.Enddate;
			travel.Mapsnapshot = model.Mapsnapshot;
			travel.Luggage = model.Luggage;
			travel.Travelers.Clear();
			if (model.Travelers != null)
			{
				var ids = model.Travelers.Select(x => x.UserId).ToList();
				var travlers = _dbSession.AsQueryable<Entity.User>().Where(x => ids.Any(m => m == x.Id)).ToList();
				foreach (var tr in travlers)
				{
					travel.Travelers.Add(new Entity.Traveler
					{
						User = tr,
						Usertype = model.Travelers.SingleOrDefault(x => x.UserId == tr.Id).Usertype
					});
				}
			}
			_dbSession.Delete<Entity.MapPoint>(travel.MapPoints.Where(x => x.Id != 0).ToList());
			travel.MapPoints.Clear();
			foreach (var point in model.MapPoints)
			{
				travel.MapPoints.Add(new Entity.MapPoint
				{
					Latitude = point.Latitude,
					Longitude = point.Longitude,
					Name = point.Name
				});
			}
			return travel;
		}

		public IEnumerable<Data.Travel> GetAll()
		{
			return _dbSession.AsQueryable<Travel.Entity.Travel>()
				.Include(x => x.Travelers)
				.Include(x => x.Travelers.Select(m => m.User))
				.Include(x => x.MapPoints)
				.ToList()
				.Map<Travel.Entity.Travel, Data.Travel>(_mapper.Map<Travel.Entity.Travel, Data.Travel>);
		}

		public bool PutTraveler(int DriverId, int TravelId, int UserId)
		{
			using (var trance = _dbSession.BeginTransaction())
			{
				var user = _dbSession.AsQueryable<Entity.User>()
					.SingleOrDefault(x => x.Id == UserId);
				if(user!=null)
				{
					var travel = _dbSession.AsQueryable<Entity.Travel>()
						.Include(x=>x.Travelers)
						.SingleOrDefault(x => x.Id == TravelId && x.Travelers.Any(m=>m.UserId == DriverId && m.Usertype == UserType.Driver));
					if(travel!=null )
					{
						travel.Travelers.Add(new Entity.Traveler
						{
							User = user,
							Usertype = UserType.Passenger
						});
						_dbSession.Save(travel);
						trance.Commit();
						return true;
					}
				}
			}
			return false;
		}


		public IEnumerable<Data.Travel> GetTravelsByIds(IEnumerable<int> Ids)
		{
			return _dbSession.AsQueryable<Travel.Entity.Travel>()
				.Include(x => x.Travelers)
				.Include(x => x.Travelers.Select(m => m.User))
				.Include(x => x.MapPoints)
				.Where(x => Ids.Contains(x.Id))
				.ToList()
				.Map<Travel.Entity.Travel, Data.Travel>(_mapper.Map<Travel.Entity.Travel, Data.Travel>);
		}
	}
}
