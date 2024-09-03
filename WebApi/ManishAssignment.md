## Company Controller
### Get
Added a exception if the result is not found
### Post
Added an exception in case the resource already exists. Otherwise create the company as long as it has CompanyCode and SiteId
### Put
Check for existing company, If there is a existing company, then provided siteId should match the existing company's SiteId, otherwise throw an exception. If the company doesn't exist, then create the company the company as long as SiteID and CompanyCode is provided
### Delete
If there is not such company with this code, then throw error message with notfound.
Then, check if company has employees, if yes then throw a error saying employees need to deleted before deleting company, For that i have also added a parameter withEmployees in Delete request which allows you to delete the company with all the employees inside it. 

## Employee Controller
Side Note: I made SiteId, CompanyCode and EmployeeCode compulsary for post and put requests. It is important to maintain the integrity between companies and employees as companyname is required when getting an employee.
### Get
Added an exception if the result is not found 
### Post 
Check for exisitng employee, if found exception is thrown saying you need to use put request.
There is also validation to see if the company with provided company code and siteId exists.
If both conditions are satisfied then employee can be created.
### Put
There is validation for company to exist with provided company code.
If company exists and there is an existing employee, then they should have same company code with provided company code in the request.
If all is set, then site provided should also match the siteid with the existing company and existing employee. This is all because of how the database is set up, changing any of these would lead to more intricate changes in how the database class is working.
### Delete
Check if employee exists, if yes, then throw error otherwise delete the resource
---
