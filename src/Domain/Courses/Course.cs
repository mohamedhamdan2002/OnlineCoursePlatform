using Domain.Categories;
using Domain.Common;
using Domain.Common.Results;
using Domain.Courses.Enums;
using Domain.Courses.Sections;
using Domain.Enrollments;
using Domain.Identity;
using Domain.Reviews;

namespace Domain.Courses;

public sealed class Course : Entity
{
    public Guid CategoryId { get; private set; }
    public Category Category { get; set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public CourseLevel Level { get; private set; }
    public double AverageRating { get; private set; }
    public int ReviewsCount { get; private set; }
    public int StudentsCount { get; private set; }
    public string? ThumbnailUrl {  get; private set; }
    public Guid InstructorId { get; private set; }
    public User Instructor { get; set; } = null!;
    
    private readonly List<Section> _sections = [];
    public IEnumerable<Section> Sections => _sections.AsReadOnly();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();

    private Course()
    {
        
    }
    private Course(Guid id, string title, string description, decimal price, string thumbnailUrl, CourseLevel level, Guid categoryId, Guid instructorId) : base(id)
    {
        Title = title;
        Description = description;
        Price = price;
        Level = level;
        ThumbnailUrl = thumbnailUrl;
        CategoryId = categoryId;
        InstructorId = instructorId;
    }

    public static Result<Course> Create(Guid id, string title, string description, decimal price, string thumbnailUrl, CourseLevel level, Guid categoryId, Guid instructorId)
    {
        if(id == Guid.Empty)
        {
            return Result.Fail<Course>(CourseErrors.IdRequired);
        }
        if (string.IsNullOrWhiteSpace(title))
        {
            return Result.Fail<Course>(CourseErrors.TitleRequired);
        }
        if (description == string.Empty)
        {
            return Result.Fail<Course>(CourseErrors.DescriptionShouldNotEmpty);
        }
        if(price <= 0)
        {
            return Result.Fail<Course>(CourseErrors.PriceInvalid);
        }
        if (string.IsNullOrWhiteSpace(thumbnailUrl))
        {
            return Result.Fail<Course>(CourseErrors.ThumbnailUrlInvalid);
        }
        if (!Enum.IsDefined(level))
        {
            return Result.Fail<Course>(CourseErrors.LevelInvalid);
        }
        if (categoryId == Guid.Empty)
        {
            return Result.Fail<Course>(CourseErrors.CategoryIdRequired);
        }
        if (instructorId == Guid.Empty)
        {
            return Result.Fail<Course>(CourseErrors.InstructorIdRequired);
        }
        
        var course = new Course(id, title, description, price, thumbnailUrl, level, categoryId, instructorId);
        return Result.Success(course);
    }

    public Result<Section> AddSection(string title)
    {
        var order = _sections.Count + 1;
        var sectionResult = Section.Create(Guid.NewGuid(), title, order, this.Id);
        if(sectionResult.IsFailure)
        {
            return sectionResult;
        }
        _sections.Add(sectionResult.Data);
        return sectionResult;
    }

}

