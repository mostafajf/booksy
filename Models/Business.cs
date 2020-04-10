using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Beauty.Models
{
   
    public class Business : EntityBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string ProfileImage { get; set; }
        public string Logo { get; set; }
        public bool IsClosed { get; set; }
        public bool IsActive { get; set; }
        public int Stars { get; set; }
        public List<SlideImage> Images { get; set; }
        public BsonDocument Address { get; set; }
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; set; }
        public List<OperationHour> OperationHours { get; set; }
        public List<TeamMember> TeamMembers { get; set; }
        public List<BusinessService> Services { get; set; }
        public List<BusinesssReview> Reviews { get; set; }
        public string CategoryId { get; set; }

    }
    public class OperationHour
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Day { get; set; }
    }
    public class TeamMember
    {
        public string Name { get; set; }
        public string Profession { get; set; }
    }
    public class BusinessService
    {
        public ServiceType ServiceType { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public string MinTimes { get; set; }
        public decimal MinPrice { get; set; }
    }
    public enum ServiceType
    {
        MakeUp,
        HairStyle,
        Brows,
        Nail,
        Tatoo
    }
    public class SlideImage
    {
        public string Caption { get; set; }
        public int Order { get; set; }
        public string ImageUrl { get; set; }
    }
    public class BusinesssReview
    {
        public string UserId { get; set; }
        public string BusinnessId { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string DateCreated { get; set; }
        public int Rate { get; set; }
        public string Review { get; set; }
    }
    public class SocialMedia
    {
        public string Instagram { get; set; }
        public string Telegram { get; set; }
    }
}
