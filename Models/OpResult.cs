using System;
using System.Collections.Generic;
using System.Text;

namespace Models;

public class OpResult<T>
{
    public T? Result { get; set; }

    public List<string> MessageList { get; set; }

    public string Message
    {
        get
        {
            var sb = new StringBuilder();
            foreach (var message in this.MessageList)
            {
                sb.AppendLine(message);
            }

            return sb.ToString();
        }
    }

    public OpResult()
    {
        this.MessageList = new List<string>();
    }

    public OpResult(T? result, string message) : this()
    {
        this.Result = result;
        AddMessage(message);
    }

    public void AddMessage(string message)
    {
        if (!string.IsNullOrEmpty(message))
        {
            this.MessageList.Add(message);
        }
    }
}