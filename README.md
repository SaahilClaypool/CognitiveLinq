# LINQ to Azure Cognitive Search

```cs
var (search, filter) =
    SearchClient<Person>
        .Search(p => p.FirstName == "Sam", p => Like(p.LastName, "Test"))
        .Filter(p => p.Courses.Any(course => course.Grade > 69))
/*
    Search: FirstName:("Sam") AND LastName:(Test)
    Filter: courses/any(course: course/grade gt 69)
*/
```