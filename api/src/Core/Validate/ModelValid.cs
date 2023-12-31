namespace SistemaVendasApi.Core.Validate;

public enum ValidType
{
    Info,
    Warning,
    Error
}

public enum ActionType
{
    Insert,
    Update,
    Delete
}

public class ModelValid
{
    public ValidType Type {get;set;} = ValidType.Info;
    public string Message {get;set;} = "";
}

public class Validation
{
    private List<ModelValid> lst = new List<ModelValid>();
    private bool hasError = false;
    private bool hasWarning = false;

    public bool HasError
    {
        get
        {
            return hasError;
        }
    }

    public bool HasWarning
    {
        get
        {
            return hasWarning;
        }
    }

    public void Add(ModelValid model)
    {
        if ((!hasError) && model.Type.Equals(ValidType.Error))
        {
            hasError = true;
        }

        if ((!hasWarning) && model.Type.Equals(ValidType.Warning))
        {
            hasWarning = true;
        }

        lst.Add(model);
    }

    public List<ModelValid> GetValids
    {
        get
        {
            return lst;
        }
    }

    public string GetErrorMessages()
    {
        string messages = "";
        foreach (ModelValid v in lst)
        {
            if (v.Type.Equals(ValidType.Error)) messages += $"\n\r{v.Message}";
        }

        return messages;
    }
    public string GetWarningMessages()
    {
        string messages = "";
        foreach (ModelValid v in lst)
        {
            if (v.Type.Equals(ValidType.Warning)) messages += $"\n\r{v.Message}";
        }

        return messages;
    }

    public string GetInfoMessages()
    {
        string messages = "";
        foreach (ModelValid v in lst)
        {
            if (v.Type.Equals(ValidType.Info)) messages += $"\n\r{v.Message}";
        }

        return messages;
    }

    public string GetAllMessages()
    {
        string messages = "";
        foreach (ModelValid v in lst)
        {
            messages += $"\n\r{v.Message}";
        }

        return messages;
    }
}