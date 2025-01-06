# SHOES STORE MANAGER

---

# PROJECT OVERVIEW - MILESTONE 03

## Team Member

| No  | Student Id | Name             |
|-----|------------|------------------|
| 1   | 22120430   | Lê Hoàng Việt    |
| 2   | 22120433   | Lê Quang Vinh    |
| 3   | 22120435   | Nguyễn Quốc Vinh |

## Introduction

The Shoes Store Manager is a comprehensive retail management solution designed for small fashion shops specializing in footwear. This project, developed as part of CSC 13001 - Windows Application Programming course, aims to digitize and streamline key business operations.


## Technology

### Frontend Architecture
- WinUI 3 Framework
- C# .NET 7 Platform
- MVVM Architectural Pattern
- Template Studio for WinUI

### Backend Infrastructure
- PostgreSQL Database System


## Implementation Milestones

### Summary

- Create Order: Develop a feature that allows users to place and confirm orders, including selecting products, specifying quantities, and providing delivery details.
- MailMessage using System.Net.Mail: Implement an automated email-sending functionality that uses the System.Net.Mail namespace to send order confirmation and related information directly to the customer's registered email address.
- Import User & Product Data by Excel using EPPlus: Create a utility that enables administrators to import large datasets of user and product information from Excel files into the system database, utilizing the EPPlus library for seamless data processing.
- Reset Password: allow users to create a new password if they forget the current one.
- Logout Feature: Implement a user-friendly logout feature that securely ends the user's session, ensuring their account remains protected from unauthorized access.
- Upgrade UI: Enhance the user interface by improving its design, responsiveness, and usability to provide a better overall experience across different devices and screen sizes. 
But some pages that are not yet fully responsive.

### Details

1. **Architecture**
   - MVVM Pattern: The core structure of the project, separating Business Logic and UI. Components are built based on MVVM Toolkit patterns.

2. **UI/UX**
   - Interface designed using Template Studio with Navigation Bar
   - Responsive not yet completed  
   - Supports system-based dark/light mode

3. **Advanced Features**

   - **Cloudinary Integration**: 
     - Utilized Cloudinary for image storage and management, allowing for efficient handling of shoes images. 
     - Supports image upload, transformation, and delivery, ensuring optimized performance and high-quality visuals.
   
   - **AdaptiveGridView Control**: 
     - Implemented AdaptiveGridView control to create a responsive and adaptive layout for displaying Shoes listings. 
     - Automatically adjusts the number of columns based on the available screen size, providing a seamless user experience across different devices.
   
   - **Mediator Pattern**: 
     - Applied the Mediator pattern to manage complex interactions between UI components and business logic. 
     - Helps in decoupling components, promoting a more maintainable and scalable architecture by centralizing communication and reducing dependencies.
     - For example, when a new category is added, an event is raised to notify the Navigation component in the Shell page to update accordingly.
   
   - **OxyPlot Integration**: 
     - Integrated OxyPlot for creating interactive and visually appealing charts and graphs. 
     - Used to display various statistics and data visualizations in the dashboard.

   - **Replay Command**: 
     - Implemented a Replay Command feature to allow users to re-execute previous commands or actions. 
     - This feature enhances user productivity by enabling quick repetition of frequent tasks without the need to manually re-enter commands.

   - **MailMessage in using System.Net.Mail**  
      - Utilized `MailMessage` from the `System.Net.Mail` namespace for sending automated emails.  
      - Includes features like sending order confirmations, password reset links, and promotional updates to customers.  
      - Ensures secure and reliable email delivery by integrating SMTP configurations.  

   - **ExcelPackage in EPPlus**  
      - Leveraged `ExcelPackage` from the EPPlus library to import and export user and product data via Excel files.  
      - Allows bulk updates and streamlined data management through pre-defined Excel templates.  
      - Supports advanced formatting, validation, and error handling for data consistency.  

   - **ApplicationDataContainer in Windows.Storage**  
      - Integrated `ApplicationDataContainer` for managing local application settings and preferences.  
      - Used to store user credentials securely for the “Remember Me” feature and maintain user preferences, like dark mode and retain user information during login sessions.  
      - Provides a lightweight and efficient way to persist small amounts of data across app sessions.  

   - **Action to ShowDialogRequest**  
      - Implemented `Action` delegates to handle `ShowDialogRequest` events for displaying modal dialogs.  
      - Enables reusable and dynamic dialog management across the application, reducing boilerplate code.  
      - Example usage includes displaying confirmation dialogs for actions such as successfully updating orders or user information, as well as for password confirmation.  


4. **Quality Assurance**
   - Manual test features before merging into main codebase
   - Excel: https://docs.google.com/spreadsheets/d/1w6y4gbz1clynGxo2AEWpsCTx4LTFJWBn/edit?usp=sharing&ouid=102239371983226166927&rtpof=true&sd=true

5. **Sources**
   - Link repository: [Keruedu/WinUI-store](https://github.com/Keruedu/WinUI-store)
   - Latest branch: `feature/update-ui`
   - Video: 



## Team work

### Task Distribution (4 hours per member)
- Summary from Meeting Minutes Below

#### Summary from Meeting Minutes

| Week | Task                                                                                       | Lê Hoàng Việt | Lê Quang Vinh | Nguyễn Quốc Vinh |
|------|--------------------------------------------------------------------------------------------|---------------|---------------|------------------|
| 1    | Initial UI design for Create Order, User, and Shoes                                        |   1 hours     |               |                  |
| 1    | Implement MailMessage to send order confirmation emails                                    |               |               |    0.5 hours     |
| 1    | Research and implement Excel import for user & product data using EPPlus                   |               |   1 hours     |                  |
| 1    | Upgrade UI for the dashboard and sidebar navigation                                        |   1 hours     |               |                  |
| 2    | Add password reset functionality                                                           |               |               |    0.5 hours     |
| 2    | Implement logout feature                                                                   |               |               |    0.5 hours     |
| 2    | Research and build ApplicationDataContainer integration                                    |   1 hours     |               |                  |
| 3    | Integrate functions for importing product categories                                       |               |   1 hours     |                  |
| 3    | Automate testing for MailMessage email functionality                                       |               |               |    0.5 hours     |
| 3    | Prepare UI for product filtering options                                                   |               |               |    1 hour        |
| 3    | Document the setup and usage of EPPlus for importing data                                  |               |   1 hours     |                  |
| 4    | Write final test cases and debugging documentation                                         |   1 hours     |               |                  |
| 4    | Prepare the final for online demo and project report (README.md)                           |               |    1 hours    |                  |
| 4    | Fix remaining UI and data binding bugs, draw C4 diagram                                    |               |               |     1 hour       |

1. **Tool**
   - Messenger, Drive, Git, Google meet, Figma

2. **Work Flow**
   - Team meets regularly at 3PM every Thursday to report progress and divide tasks
   - Project materials and references are uploaded to Drive or pinned in the team's private Messenger group
   - Meeting minutes: [Google Docs - Vietmamese](https://docs.google.com/document/d/1aqO8fGVvLPGIK6Ay-KMkrwZ34DUbOCRPOZugCG_Anyo/edit?usp=sharing)

3. **Git Flow**
   - Using 1 shared repository for the team, members use feature branches
   - After features are completed, they are manual tested. After passing tests, members create pull requests for review by other members. After consensus, code is merged into main branch during regular meetings
   - Repository link: [Keruedu - WinUI-store](https://github.com/Keruedu/WinUI-store)
   - Doxygen: [Doxygen.zip - Drive](https://drive.google.com/file/d/19BP5doNEaM5YbCftgrKwOZ_DwXAbH4ch/view)
   - C4 Diagram: [C4Diagram - Figma](https://www.figma.com/design/PWMPWzKTRswU7ngkBfr8WJ/The-C4-model-for-Figma-(Community)?node-id=124-108&t=sRnsadHQHHgILbbB-1)

## Self-Assessment Scores

| No  | Student Id | Name             | Score        | Hours        |
|-----|------------|------------------|--------------|--------------|
| 1   | 22120430   | Lê Hoàng Việt    | 10           | 4            |
| 2   | 22120433   | Lê Quang Vinh    | 10           | 4            |
| 3   | 22120435   | Nguyễn Quốc Vinh | 10           | 4            |
