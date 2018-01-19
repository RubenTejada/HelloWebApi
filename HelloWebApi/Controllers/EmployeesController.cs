using HelloWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;

namespace HelloWebApi.Controllers
{
    public class EmployeesController : ApiController
    {

        private static IList<Employee> list = new List<Employee>()
        {
            new Employee()
            {
                Id=12345,
                FirstName="John",
                LastName="Human",
                Department=2
            },

            new Employee()
            {
                Id=12346,
                FirstName="Jane",
                LastName="Public",
                Department=3
            },

            new Employee()
            {
                Id=123457,
                FirstName="Joseph",
                LastName="Law",
                Department=2
            }
        };


        //Get api/employees
        public IEnumerable<Employee> GetAllEmployees()
        {
            return list;
        }

        ////Get api/employees/12345
        //public Employee GetEmployee(int id)
        //{
        //    return list.SingleOrDefault(e => e.Id == id);
        //}


        ////Post api/employees
        //public void PostEmployee(Employee employee)
        //{
        //    int maxId = list.Max(e => e.Id);
        //    employee.Id = maxId + 1;

        //    list.Add(employee);
        //}

        //Put api/employees
        public void PutEmployee(Employee employee)
        {
            int updateIndex = list.ToList().FindIndex(e => e.Id == employee.Id);
            list[updateIndex] = employee;
        }


        //Delete api/employees
        public void DeleteEmployee(int id)
        {
         //   Employee employee = Get(id);
          //  list.Remove(employee);
        }


        public HttpResponseMessage Get(int id)
        {
            var employee = list.SingleOrDefault(e => e.Id == id);


            if (employee == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }                          

            return Request.CreateResponse<Employee>(employee);

        }

        public IEnumerable<Employee> GetByDepartment(int department)
        {
            int[] validDepartments = { 1, 2, 3, 5, 8, 13 };

            if (!validDepartments.Any(d => d == department))
            {
                var response = new HttpResponseMessage()
                {
                    StatusCode = (HttpStatusCode)422,//Unprocessable Entity
                    ReasonPhrase = "Invalid Department"
                };

                throw new HttpResponseException(response);
            }


            return list.Where(e => e.Department == department);

        }


        //public IEnumerable<Employee> Get([FromUri]Filter filter)
        //{
        //    return list.Where(e => e.Department == filter.Department && e.LastName == filter.LastName);
        //}


        public HttpResponseMessage Post(Employee employee)
        {
            int maxID = list.Max(e => e.Id);
            employee.Id = maxID + 1;
            list.Add(employee);

            var response = Request.CreateResponse<Employee>(HttpStatusCode.Created, employee);

            string uri = Url.Link("DefaultApi", new { id = employee.Id });

            response.Headers.Location = new Uri(uri);

            return response;

        }


        public HttpResponseMessage Patch(int id, Delta<Employee> deltaEmployee)
        {
            var employee = list.SingleOrDefault(e => e.Id == id);

            if(employee ==null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            deltaEmployee.Patch(employee);

            return Request.CreateResponse(HttpStatusCode.NoContent);
        }

    }
}
