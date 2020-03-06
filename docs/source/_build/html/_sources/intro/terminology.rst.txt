Terminology
===========

The documentation and object model use a certain terminology that you should be aware of.

Applications
^^^^^^^^^^^^

Allows you to manage multiple different software projects, for example. Each application has its own unique set of roles and delegations.

Roles
^^^^^^^

Are similar to security groups , to which users can become members and acquire a level of security that gives them the ability to permform some bussines operations. Roles can contains permissions, subjects and mappings. A role could be a teacher, custodian, student, etc.

Permissions
^^^^^^^^^^^

A permision is the ability to perform some specific operation, like view grades, edit grades, etc.

Subjects
^^^^^^^^

Subjects (Users) are grouped into roles an each defined role access permissions based upon the role not individual.

Mappings
^^^^^^^^

Roles comming from authentication system can be mapped to the application roles.

Store
^^^^^

A mechanisim to allow you to store persistent the Balea's object model such us applications, roles, permissions, delegations. Balea provides out of the box two stores:

    * ASP.NET Core JSON Configuration Provider.
    * Entity Framework Core. 