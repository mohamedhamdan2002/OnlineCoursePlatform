using Domain.Common;
using Domain.Common.Results;
using Domain.Courses.Enums;
using Domain.Courses.Sections;
using Microsoft.EntityFrameworkCore;

namespace Domain.Courses.Lectures;

public sealed class Lecture : Entity
{
    public string Title { get; private set; }
    public int Order { get; private set; }
    public string? VideoUrl { get; private set; }
    public VideoStatus VideoStatus { get; private set; }
    public DateTime Duration { get; private set; }
    public bool IsPreview { get; private set; }
    public Guid SectionId { get; private set; }
    public Section Section { get; set; } = null!;
    
    

    private Lecture()
    {
    }
    private Lecture(Guid id, string title, int order, Guid sectionId) 
        : base(id)
    {
        Title = title;
        Order = order;
        SectionId = sectionId;
    }
    //private Lecture(Guid id, string title, Guid sectionId, string videoUrl, DateTime duration, bool isPreview)
    //    : this(id, title, sectionId)
    //{
    //    VideoUrl = videoUrl;
    //    Duration = duration;
    //    IsPreview = isPreview;
    //}

    public static Result<Lecture> Create(Guid id, string title, int order, Guid sectionId)
    {
        if(id == Guid.Empty)
        {
            return Result.Fail<Lecture>(LectureErrors.IdRequired);
        }
        if(string.IsNullOrWhiteSpace(title))
        {
            return Result.Fail<Lecture>(LectureErrors.TitleRequired);
        }
        
        if (sectionId == Guid.Empty)
        {
            return Result.Fail<Lecture>(LectureErrors.SectionIdRequired);
        }
        return Result.Success(new Lecture(id, title, order, sectionId));
    }

    public Result UpdateVideoUrl(string videoUrl)
    {
        if(string.IsNullOrWhiteSpace(videoUrl))
        {
            return Result.Fail(LectureErrors.VideoUrlInvalid);
        }
        VideoUrl = videoUrl;
        return Result.Success();
    }
    //public static Result<Lecture> Create(Guid id, string title, Guid sectionId, string videoUrl, DateTime duration, bool isPreview)
    //{
    //    if (id == Guid.Empty)
    //    {

    //    }
    //    if (string.IsNullOrWhiteSpace(title))
    //    {

    //    }

    //    if (sectionId == Guid.Empty)
    //    {

    //    }
    //    if (string.IsNullOrWhiteSpace(videoUrl))
    //    {

    //    }
    //    return Result.Success(new Lecture(id, title, sectionId, videoUrl, duration,isPreview));
    //}
}
