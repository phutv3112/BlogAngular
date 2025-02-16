namespace BlogAngular.Api.Models
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        //public DateTime CreatedDate { get; set; } = DateTime.Now;
        //public DateTime UpdatedDate { get; set; } = DateTime.Now;
        public DateTime CreatedDate { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        public DateTime UpdatedDate { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));

    }
}
