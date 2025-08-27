
# Plus:

Frontend:

- Improve the UI/UX Layout with responsive design
  
<img width="904" height="547" alt="newfrontend" src="https://github.com/user-attachments/assets/039dd287-cecb-4934-a044-db5179f3cd9b" />

Backend:

- Add unit tests for the calculation logic (Controller and Services)

<img width="2507" height="715" alt="unittests" src="https://github.com/user-attachments/assets/26b09575-2a16-4fcd-a31c-f1b2f7549775" />

- Add error handling for invalid inputs
- Add custom validation attributes if necessary
- Add dependency injection for better testability
- Separation of controller logic from business logic (Service Layer)
- Getting configuration rate seetings from appsettings.json

# Technical Task:

You are finishing implementing a Commission Calculator.

There is an API in place and a react application.
The controller is created but the code is unfinished.

Your job is to finish the technical task:
 - Connect the backend and frontend
 - Implement the calculation in the C# controller

Develop this at **production quality**.

The code is imcomplete on purpose, it's a simulation of how most work is done, good luck:

# Business rules

 At FCamara, we pay 20% commission for Local Sales and 35% commission on Foreign Sales.
 Our competitons only pay 2% commission and 7.55% on Foreign Sales.

Example:
- Local Sales count: 10
- Foreign Sales Count: 10
- Averaga Sales Amount: £100

FCamara Commission: £550
    Local Sales Commission = 20% * 10 sales * 100 average sale
    Foreign Sales Commission = 35% * 10 sales * 100 average sales
Competitor Commission: £95.5
    Local Sales Commission = 2% * 10 sales * 100 average sale
    Foreign Sales Commission = 7.55% * 10 sales * 100 average sales
