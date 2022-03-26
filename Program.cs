using System;
using System.Text.Json;

const string jsonFile = "students.json";
const int tuitionFee = 10000;
char useraction = '1';
while (useraction != 'q')
{
    Console.WriteLine(@"Choose an option from the following list:
                        a - Add a student
                        r - Remove a student
                        p - Print list of students
                        t - Print student tuitions
                        q - Quit
                        Your option?");
    useraction = Convert.ToChar(Console.ReadLine());
    switch (useraction) {
        case 'a':
          AddStudent();
          break;
        case 'r':
          RemoveStudent();
          break; 
        case 'p':
          PrintStudentList();
          break;
        case 't':
          PrintStudentTuitionsList();
          break;    
    }
}

static void AddStudent() {
    string name = "";
    int age = 0;
    while (name == "") {
        Console.WriteLine("Please enter student's name:");
        name = Console.ReadLine();
        if (name == "") {
            Console.WriteLine("Student can't be nameless, try again");
        }
    }
    while (age == 0) {
        Console.WriteLine("Please enter student's age:");
        try {
            age = int.Parse(Console.ReadLine());
        }
        catch (Exception e) {
            Console.WriteLine("Age has to be a number, try again");
        }
    }
    Student newStudent = new Student(name, age);
    List<Student> studentList = Deserialize();
    foreach (Student student in studentList) {
        if (newStudent.Name == student.Name) {
            Console.WriteLine($"That student already exists, try again");
            return;
        }
    }
    studentList.Add(newStudent);
    Serialize(studentList);
}

static void RemoveStudent() {
    bool isRemovable = false;
    Console.WriteLine("Please enter the students name:");
    string name = Console.ReadLine();
    List<Student> studentList = Deserialize();
    foreach (Student student in studentList) {
        if (student.Name == name) {
            isRemovable = studentList.Remove(student);
            Serialize(studentList);
            return;
        }
    }
    Console.WriteLine("That student doesn't exist");
}

static void PrintStudentList() {
    List<Student> studentList = Deserialize();
    foreach (Student student in studentList) {
        Console.WriteLine($"{student}");
    }
}

static void PrintStudentTuitionsList() {
    List<Student> studentList = Deserialize();
    var discountCheck = (int age) => {
        if (age >= 25) {
            return (tuitionFee);
        }
        else {
            return (tuitionFee / 10 * 9);
        }
    };
    foreach (Student student in studentList) {
        Console.WriteLine($"{student} needs to pay {discountCheck(student.Age)}");
    }
}

static List<Student> Deserialize() {
    string jsonString = File.ReadAllText(jsonFile);
    List<Student> studentList = JsonSerializer.Deserialize<List<Student>>(jsonString);
    return studentList;
}

static void Serialize(List<Student> studentList) {
    string jsonString = JsonSerializer.Serialize(studentList);
    File.WriteAllText(jsonFile, jsonString);
}