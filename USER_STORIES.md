# Wiki Project User Stories & Implementation Guide

This document lists the core user stories for the AjpWiki system and suggests where their implementations should reside in the codebase, following DDD and the project structure.

## User Stories

1. **Account Creation**
   - *As a new user, I want to create an account so that I can contribute to the wiki.*
   - **Implementation:** `AjpWiki.Application/Services/IUserService.cs`, `AjpWiki.Infrastructure/Services/UserService.cs`, `AjpWiki.Web/Pages/Users/`

2. **Access Request & Approval**
   - *As a user, I want to request access so that an admin can approve my participation.*
   - *As an admin, I want to approve or reject user requests so that only trusted users can edit.*
   - **Implementation:** `AjpWiki.Application/Services/IUserService.cs`, `AjpWiki.Infrastructure/Services/UserService.cs`, `AjpWiki.Web/Pages/Users/`

3. **Wiki Page Editing**
   - *As a user, I want to edit a wiki page so that I can improve its content.*
   - **Implementation:** `AjpWiki.Application/Services/IWikiArticleService.cs`, `AjpWiki.Infrastructure/Services/WikiArticleService.cs`, `AjpWiki.Web/Pages/Articles/`

4. **Versioning**
   - *As a user, I want each edit to create a new version so that changes are tracked.*
   - **Implementation:** `AjpWiki.Domain/Entities/Articles/WikiArticleVersion.cs`, `AjpWiki.Application/Services/IWikiArticleService.cs`

5. **Change History & Rollback**
   - *As a user, I want to view the change history so that I can see what was changed and by whom.*
   - *As a user, I want to roll back to a previous version so that I can undo unwanted changes.*
   - **Implementation:** `AjpWiki.Web/Pages/Articles/`, `AjpWiki.Application/Services/IWikiArticleService.cs`

6. **Component Editing**
   - *As a user, I want to add components (e.g., images, tables) to a page while editing so that articles are richer.*
   - **Implementation:** `AjpWiki.Domain/Entities/Articles/ArticleComponent.cs`, `AjpWiki.Web/Pages/Articles/`

7. **Discussion/Chat**
   - *As a user, I want to view and participate in discussions about a page so that I can collaborate with others.*
   - **Implementation:** `AjpWiki.Domain/Entities/Articles/`, `AjpWiki.Web/Pages/Articles/`

8. **Change Review**
   - *As a user, I want to review and accept or reject proposed changes so that only quality edits are published.*
   - **Implementation:** `AjpWiki.Application/Services/IWikiArticleService.cs`, `AjpWiki.Web/Pages/Articles/`

9. **Search & Navigation**
   - *As a user, I want to search for articles so that I can quickly find information.*
   - *As a user, I want to browse articles by category or tag so that I can explore related topics.*
   - **Implementation:** `AjpWiki.Application/Services/IWikiArticleService.cs`, `AjpWiki.Web/Pages/Articles/`

10. **Article Creation**
    - *As a user, I want to create new articles so that I can add new topics.*
    - **Implementation:** `AjpWiki.Application/Services/IWikiArticleService.cs`, `AjpWiki.Web/Pages/Articles/`

11. **Profile Management**
    - *As a user, I want to edit my profile (name, email, password, avatar) so that my information is up to date.*
    - *As a user, I want to delete my account so that I can leave the platform if I choose.*
    - **Implementation:** `AjpWiki.Application/Services/IUserService.cs`, `AjpWiki.Web/Pages/Users/`

12. **Role & Permission Management**
    - *As an admin, I want to manage user roles and permissions so that I can control access.*
    - **Implementation:** `AjpWiki.Application/Services/IRoleService.cs`, `AjpWiki.Infrastructure/Services/RoleService.cs`, `AjpWiki.Web/Pages/Users/`

13. **Notifications**
    - *As a user, I want to receive notifications about relevant activity so that I stay informed.*
    - **Implementation:** `AjpWiki.Application/Services/INotificationService.cs`, `AjpWiki.Web/Pages/`

14. **Password Reset**
    - *As a user, I want to reset my password if I forget it so that I can regain access.*
    - **Implementation:** `AjpWiki.Application/Services/IUserService.cs`, `AjpWiki.Web/Pages/Users/`

15. **Tagging/Categorization**
    - *As a user, I want to tag or categorize articles so that content is organized.*
    - **Implementation:** `AjpWiki.Domain/Entities/Articles/`, `AjpWiki.Web/Pages/Articles/`

---

> Refer to this file when planning features or assigning implementation work. Update as new stories are added or requirements change.
