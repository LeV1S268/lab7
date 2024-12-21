using System;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using AnimalLibrary;

namespace lab07
{
    class Program
    {
        static void Main()
        {
            // Генерация XML-документа
            var xmlDoc = new XDocument(new XElement("Classes"));

            // Загрузка сборки библиотеки
            Assembly assembly = typeof(Animal).Assembly; // Получаем сборку, содержащую классы из библиотеки

            // Перебор всех типов в библиотеке
            foreach (var type in assembly.GetTypes())
            {
                if (!type.IsClass) continue; // Игнорируем не классы

                // Создаём элемент для класса
                var classElement = new XElement("Class",
                    new XAttribute("Name", type.Name));

                // Проверяем, есть ли пользовательский атрибут
                var attribute = type.GetCustomAttribute<CustomCommentAttribute>();
                if (attribute != null)
                {
                    classElement.Add(new XAttribute("Comment", attribute.Description));
                }

                // Добавляем свойства класса
                foreach (var property in type.GetProperties())
                {
                    classElement.Add(new XElement("Property", property.Name));
                }

                // Добавляем методы класса
                foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
                {
                    classElement.Add(new XElement("Method", method.Name));
                }

                // Добавляем класс в XML-документ
                xmlDoc.Root.Add(classElement);
            }

            // Сохранение XML в файл
            string filePath = "LibraryClasses.xml";
            xmlDoc.Save(filePath);

            Console.WriteLine($"XML файл успешно создан: {filePath}");
        }
    }
}