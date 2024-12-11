# SHOES STORE MANAGER

---

# PROJECT OVERVIEW - MILESTONE 02

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

- Database design and connection
- Add, delete, update, view details with pagination, filter, sort for Category, Shoes, Order, User 
- Dashboard page showing order statistics, inventory count, recent orders, best-selling products and order analysis charts by month/year
- UI development for pages (with dark mode support)

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

4. **Quality Assurance**
   - Manual test features before merging into main codebase
   - Excel: https://docs.google.com/spreadsheets/d/1T_RbcEXHIQG2JNZaKT1nggJXLqoa-s91/edit?usp=drive_link&ouid=112986944329988206623&rtpof=true&sd=true

5. **Sources**
   - Link repository: [Keruedu/WinUI-store](https://github.com/Keruedu/WinUI-store)
   - Latest branch: `main`
   - Video: https://drive.google.com/file/d/1k2opjWWgXxYvLEe0eVdqnTB1wrbuddeq/view?usp=drive_link
   - Doxygen: https://drive.google.com/file/d/1eM3VkJ1wNAjVmxo7ju2KWT61nmrby-Dj/view?usp=drive_link

## Team work

### Task Distribution (4 hours per member)
- Summary from Meeting Minutes Below

#### Summary from Meeting Minutes

| Week | Task                                                                                       | Lê Hoàng Việt | Lê Quang Vinh | Nguyễn Quốc Vinh |
|------|--------------------------------------------------------------------------------------------|---------------|---------------|------------------|
| 1    | Initial UI design for User, Order, Shoes                                                   | 1 hours       |               |                  |
| 1    | Write necessary Data Services for Dashboard                                                |               | 0.5 hours     |                  |
| 1    | Research and build Cloudinary Service                                                      |               |               | 0.5 hours        |
| 2    | Display user details on click                                                              | 0.5 hours     |               |                  |
| 2    | Add filtering options for User                                                             | 0.5 hours     |               |                  |
| 2    | Add filtering options for Shoes and Order                                                  |               | 0.5 hours     |                  |
| 2    | Research OxyPlot                                                                           |               |               | 1 hours          |
| 2    | Build Auth Service and LoginControl                                                        |               |               | 0.5 hours        |
| 3    | Integrate functions for Add/Update/Ban/Unban User                                          | 1 hours       |               |                  |
| 3    | Fix display issues and list bugs                                                           | 0.5 hours     |               |                  |
| 3    | Complete Dashboard UI                                                                      |               | 0.5 hours     |                  |
| 3    | Prepare report and demo video                                                              |               |  0.5 hours    |                  |
| 3    | Design category list on navigation bar using Mediator                                      |               |               | 1 hours          |
| 3    | Write manual test documentation                                                            |               |               | 0.5 hours        |
| 4    | Prepare report and demo video                                                              | 0.5 hours     |               |                  |
| 4    | Write manual test documentation                                                            | 0.5 hours     |               |                  |
| 4    | Fix display issues and list bugs                                                           | 0.5 hours     |               |                  |
| 4    | Prepare report and demo video                                                              |               | 1 hours       |                  |
| 4    | Write manual test, Dioxegen documentation                                                  |               | 1 hours       |                  |
| 4    | Fix display issues and list bugs                                                           |               |               | 0.5 hours        |

1. **Tool**
   - Messenger, Drive, Git, Google meet

2. **Work Flow**
   - Team meets regularly at 3PM every Thursday to report progress and divide tasks
   - Project materials and references are uploaded to Drive or pinned in the team's private Messenger group
   - Meeting minutes: [Google Docs - Vietmamese](https://docs.google.com/document/d/1aqO8fGVvLPGIK6Ay-KMkrwZ34DUbOCRPOZugCG_Anyo/edit?usp=drive_link)

3. **Git Flow**
   - Using 1 shared repository for the team, members use feature branches
   - After features are completed, they are manual tested. After passing tests, members create pull requests for review by other members. After consensus, code is merged into main branch during regular meetings
   - Repository link: [Keruedu/WinUI-store](https://github.com/Keruedu/WinUI-store)

## List of Errors

1. Cannot delete Shoes when they are in OrderDetail due to foreign key constraint.
2. Cannot delete Category when it has associated Shoes.
3. Some filter functions on various pages are not working.
4. Adding User does not include setting the password.
5. Pagination issues: Filters reset when navigating to a different page.
6. Some parts of the page do not update when data changes.

## Self-Assessment Scores

| No  | Student Id | Name             | Score        | Hours        |
|-----|------------|------------------|--------------|--------------|
| 1   | 22120430   | Lê Hoàng Việt    | 10           | 4            |
| 2   | 22120433   | Lê Quang Vinh    | 10           | 4            |
| 3   | 22120435   | Nguyễn Quốc Vinh | 10           | 4            |
