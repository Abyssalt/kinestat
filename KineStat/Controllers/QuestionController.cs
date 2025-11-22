using Microsoft.AspNetCore.Mvc;
using KineStat.Models;

namespace KineStat.Controllers
{
    public class QuestionController : Controller    // TODO : Adjust this class when the db will be linked
    {
        private static List<Question> _questions = new List<Question>() // Temporary list of question while waiting for the DB, replace it with the DbContext
        {
            new QuestionBool { Id = 1, Title = "Test", CategoryId = 1 } // Test question, used to test GetQuestions (at https://localhost:7133/Question/GetQuestions)
        };

        public IActionResult Index()
        {
            return View(_questions);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string questionType, string title, double rvPositif, double rvNegatif, int? clusterId, int bilanId)
        {
            Question newQuestion = questionType switch
            {
                "Bool" => new QuestionBool
                {
                    Title = title,
                    RVPositive = rvPositif,
                    RVNegative = rvNegatif,
                    ClusterId = clusterId,
                    CategoryId = bilanId
                },
                "Ladder" => new QuestionLadder
                {
                    Title = title,
                    RVPositive = rvPositif,
                    RVNegative = rvNegatif,
                    ClusterId = clusterId,
                    CategoryId = bilanId
                },
                "QCM" => new QuestionQCM
                {
                    Title = title,
                    RVPositive = rvPositif,
                    RVNegative = rvNegatif,
                    ClusterId = clusterId,
                    CategoryId = bilanId
                },
                _ => null
            };

            if (newQuestion != null)
            {
                if (_questions.Count > 0)   // Give an auto-increment ID
                {
                    newQuestion.Id = _questions.Max(q => q.Id) + 1;
                }
                else
                {
                    newQuestion.Id = 1;
                }
                _questions.Add(newQuestion);
                return RedirectToAction("Index");
            }

            return View();  // If it fail, reload the page
        }

        public IActionResult Edit(int id)
        {
            var question = _questions.FirstOrDefault(q => q.Id == id);
            if (question == null)
            {
                return NotFound();  // Return "404 : Not found" page
            }
            return View(question);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string title, double rvPositif, double rvNegatif, int? clusterId, int bilanId)
        {
            var question = _questions.FirstOrDefault(q => q.Id == id);  // FirstOrDefault is a LINQ method, it's better for this kind of use
            if (question == null)
            {
                return NotFound();
            }

            question.Title = title;
            question.RVPositive = rvPositif;
            question.RVNegative = rvNegatif;
            question.ClusterId = clusterId;
            question.CategoryId = bilanId;

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var question = _questions.FirstOrDefault(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);  // Display a warning page "Etes-vous sur?"
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var question = _questions.FirstOrDefault(q => q.Id == id);
            if (question != null)
            {
                _questions.Remove(question);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)    // Give a readonly view of the selected question, don't know if it's useful, delete if it isn't
        {
            var question = _questions.FirstOrDefault(q => q.Id == id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        [HttpGet]
        public IActionResult GetQuestions(int? bilanId) // I based this on the same thing in PatientController.cs, don't know if it's useful
        {
            var questions = bilanId.HasValue
                ? _questions.Where(q => q.CategoryId == bilanId.Value).ToList()
                : _questions;

            return Json(questions);
        }
    }
}