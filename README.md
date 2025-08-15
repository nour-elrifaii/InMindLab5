# Hi Lea!

In this lab, I secure endpoints with Keycloak roles, add an Azure Blob Storage emulator for user profile images, and enable static file serving for shared assets. 

## Authentication & Authorization 
- Ran Keycloak in Docker, created a realm/client, and added users (students/teachers) with role assignments.
- Configured JWT Bearer auth in Program.cs to validate tokens issued by Keycloak using Postman.
-  Applied [Authorize(Roles = "...")] on controllers/endpoints so only users with the right role can access them.

## File Storage
- Started an Azure Blob Storage emulator in Docker and configured the connection string in the app.
- Added fields to store each user’s profile picture reference (blob name) and ran a migration.
- Implemented upload and download actions that stream files to/from the blob container.

## Static Files
- Enabled UseStaticFiles() and placed assets under wwwroot.
- University assets (logo, campus map, etc.) are now served directly via static URLs.

One of the uploaded images is so me when working on the lab, hope you approve!
