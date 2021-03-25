Terminology
===========

The documentation and object model use a certain terminology that you should be aware of.

Applications
^^^^^^^^^^^^

Allow you to manage multiple different software projects, for example. Each application has its own unique set of roles and delegations.

Roles
^^^^^^^

Are similar to security groups, to which users can become members and acquire a level of security that gives them the ability to perform some business operations. Roles can contain permissions, subjects and mappings. A role could be a teacher, custodian, student, etc.

Permissions
^^^^^^^^^^^

A permission is the ability to perform some specific operation like view grades, edit grades, etc.

Subjects
^^^^^^^^

Subjects (Users) are grouped into roles and role access permissions are based upon the role, not individual.

Mappings
^^^^^^^^

Roles coming from authentication system can be mapped to the application roles.

Store
^^^^^

A mechanism that allow you to store persistent the Balea's object model such as applications, roles, permissions, delegations. Balea provides out-of-the-box two stores:

    * ASP.NET Core JSON Configuration Provider.
    * Entity Framework Core. 
