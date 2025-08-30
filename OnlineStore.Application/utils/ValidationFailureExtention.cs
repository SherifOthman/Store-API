
using FluentValidation.Results;
using OnlineStore.Application.Common;

namespace OnlineStore.Application.utils;
public static class ValidationFailureExtention
{
    public static List<ErrorItem> ToErrorItemList(this ICollection<ValidationFailure> failures)
    {
        return failures.Select(e => new ErrorItem { Field = e.PropertyName, Message = e.ErrorMessage }).ToList();
    }
   
   
}
