using System;
using System.Collections.Generic;

namespace GeneratorApp.Model
{
    public class User
    {
        public int Id { get; set; }

        // non clustered index, name + lastname for composite and nonkey
        public string Name { get; set; }

        public string LastName { get; set; }

        // filtered non clustered index
        public DateTime? TrainingCompletionDate { get; set; }
    }
}
