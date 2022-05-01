using System.Linq.Expressions;

namespace CognitiveLinq;
public class SearchBuilder<T>
{
    public Action<string>? Log { get; set; }

    public string Search(params Expression<Func<T, bool>>[] exp)
    {
        var searches = exp.Select(Search);
        return string.Join(" AND ", searches);
    }

    private string Search(Expression<Func<T, bool>> exp)
    {
        return Visit(exp.Body);
    }

    public string Visit(Expression exp)
    {
        Log?.Invoke($"Visit {exp}");
        return exp switch
        {
            BinaryExpression bin => VisitBinary(bin),
            UnaryExpression un => VisitUnary(un),
            ConstantExpression constant => VisitConstant(constant),
            MemberExpression member => VisitMember(member),
            _ => throw new NotImplementedException()
        };
    }

    private string VisitMember(MemberExpression member)
    {
        Log?.Invoke(member.ToString());
        return string.Join("/", member.ToString().Split(".").Skip(1));
    }

    private string VisitConstant(ConstantExpression constant)
    {
        Log?.Invoke($"Visit Constant {constant}");
        return Type.GetTypeCode(constant.Value!.GetType()) switch
        {
            _ => constant.Value.ToString()!,
        };
    }

    private string VisitUnary(UnaryExpression un)
    {
        Log?.Invoke($"Visit Unary {un}");
        return un.NodeType switch
        {
            ExpressionType.Not => " NOT " + Visit(un.Operand),
            _ => throw new NotImplementedException()
        };
    }

    private string VisitBinary(BinaryExpression bin)
    {
        Log?.Invoke($"Visit Binary {bin}");
        return bin.NodeType switch
        {
            ExpressionType.Equal => VisitEqual(bin),
            _ => throw new NotImplementedException()
        };
    }

    private string VisitEqual(BinaryExpression bin)
    {
        Log?.Invoke($"Visit Equal {bin}");
        var left = Visit(bin.Left);
        var right = Visit(bin.Right);
        return $"{left}:({right})";
    }
}
