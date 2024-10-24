# PART2 MVC POE
# UploadFileToDb

## Overview

`UploadFileToDb` is an ASP.NET Core MVC application that enables users to manage employee records and upload files to a database. The application provides features for viewing, creating, editing, and deleting employee records, as well as handling file uploads and approvals.

## Features

- Employee Management:
  - View employee list
  - View employee details
  - Create new employee records
  - Edit existing employee records
  - Delete employee records

- File Upload Management:
  - Upload files with validations
  - Store file metadata in the database
  - View uploaded files

- Approval Management:
  - View approval forms
  - Approve or reject forms

## Technologies Used

- ASP.NET Core MVC
- Entity Framework Core
- In-memory database for testing
- xUnit for unit testing
- Moq for mocking dependencies

## Setup Instructions

### Prerequisites

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
- Visual Studio or any IDE of your choice

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/UploadFileToDb.git
