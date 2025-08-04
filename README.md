
KeeWoodworking Order Management System (OMS)
KeeWoodworking OMS is a backend solution designed to manage operations for a woodworking business, specifically focusing on customer account handling, order management, and system authentication. Built using ASP.NET Core and structured around the Clean Architecture principles, the system ensures scalability, maintainability, and clear separation of concerns.

At the core, the project separates its functionality into multiple layers:

Application Layer: Contains Data Transfer Objects (DTOs), service extensions, and behavioral patterns like validation, making it the heart of the system’s business logic and request handling.

Domain Layer: Houses core business entities and interfaces that define contracts without being coupled to infrastructure logic.

Infrastructure & Persistence Layers: Handle external dependencies such as database operations, identity management, and email services.

Web API Layer: Acts as the entry point, exposing endpoints to interact with the system, primarily for user registration, authentication, password recovery, token management, and email verification.

Security and user management are strongly emphasized in this system. It supports JWT-based authentication, refresh tokens, email verification, and forgot password workflows. The inclusion of DTOs like RegisterRequest, AuthenticationRequest, VerifyEmailRequest, and ForgotPasswordRequest demonstrates how granular and structured the user flow is.

Additionally, the project includes a Dockerfile, enabling easy containerization and deployment of the service. It also includes developer-centric tools and guidelines such as .editorconfig, .gitignore, a detailed README.md, and a CODE_OF_CONDUCT.md, promoting clean collaboration and development standards.

From a software engineering perspective, this project showcases best practices in .NET Core development. Its structure promotes testability, reusability, and clean scaling for future additions like inventory tracking, shipment handling, or analytics.

Overall, KeeWoodworking OMS is a robust, production-ready backend framework for managing the operational backbone of a woodworking business or any similar enterprise requiring authenticated order processing and user management.
