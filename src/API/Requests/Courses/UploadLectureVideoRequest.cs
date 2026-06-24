namespace API.Requests.Courses;

public sealed record UploadLectureVideoRequest
{
    public IFormFile VideoFile { get; init; }
}

