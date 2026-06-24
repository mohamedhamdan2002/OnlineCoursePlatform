using Domain.Common;
using Domain.Common.Results;
using Domain.Courses;
using Domain.Courses.Lectures;

namespace Domain.Courses.Sections;

public class Section : Entity
{
    public string Title { get; private set; }
    public int Order { get; private set; }   
    public Guid CourseId { get; private set; }
    public Course Course { get; set; } = null!;

    private readonly List<Lecture> _lectures = [];
    public IEnumerable<Lecture> Lectures => _lectures.AsReadOnly();
    private Section()
    {
        
    }
    private Section(Guid id, string title, int order, Guid courseId)
    : base(id)
    {
        Title = title;
        Order = order;
        CourseId = courseId;
    }
    private Section(Guid id,  string title, int order, Guid courseId, List<Lecture>  lectures)
        : this(id, title, order, courseId) 
    { 
        _lectures = lectures;
    }

    public static Result<Section> Create(Guid id, string title, int order, Guid courseId)
    {
        if (id == Guid.Empty)
        {
            return Result.Fail<Section>(SectionErrors.IdRequired);
        }

        if(string.IsNullOrWhiteSpace(title))
        {
            return Result.Fail<Section>(SectionErrors.TitleRequired);
        }

        if (order <= 0)
        {
            return Result.Fail<Section>(SectionErrors.InvalidOrder);
        }

        if (courseId == Guid.Empty)
        {
            return Result.Fail<Section>(SectionErrors.CourseIdRequired);
        }

        return Result.Success(new Section(
            id,
            title,
            order,
            courseId
        ));
    }

    public Result<Lecture> AddLecture(string title)
    {
        var order = _lectures.Count + 1;
        var lectureResult = Lecture.Create(Guid.NewGuid(), title, order, this.Id);
        if (lectureResult.IsFailure)
        {
            return lectureResult;
        }
        _lectures.Add(lectureResult.Data);
        return lectureResult;
    }
}
