using System;
using System.Collections;
using System.Collections.Generic;


public class QuestionReviewDTO 
{
    public Guid QuestionReviewID { get; set; }
    public string QuestionText { get; set; }
    public Guid CategoryID { get; set; }
    public short Language { get; set; }
    public Dictionary<string, string> AnswerMap { get; set; }
    public int CorrectAnswerKey { get; set; }
}
