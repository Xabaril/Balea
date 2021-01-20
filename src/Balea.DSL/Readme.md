# Balea.DSL

## Introduction to XACML and DSL's for Authorization Language

> https://en.wikipedia.org/wiki/XACML 

*XACML* stands for "eXtensible Access Control Markup Language". The standard defines a declarative fine-grained, attribute-based access control policy language, an architecture, and a processing model describing how to evaluate access requests according to the rules defined in policies.

The XACML model supports and encourages the separation of enforcement (PEP) from decision making (PDP) from management / definition (PAP) of the authorization. When access decisions are hard-coded within applications (or based on local machine userids and access control lists (ACLs)), it is very difficult to update the decision criteria when the governing policy changes and it is hard to achieve visibility or audits of the authorization in place. When the client is decoupled from the access decision, authorization policies can be updated on the fly and affect all clients immediately.

> PAP: Point which manages access authorization policies

> PDP: Point which evaluates access requests against authorization policies before issuing access decisions

> PEP: Point which intercepts user's access request to a resource, makes a decision request to the PDP to obtain the access decision
(i.e. access to the resource is approved or rejected), and acts on the received decision
Policy definition in XACML is done using XML, which is certainly a very difficult-to-read language. 

ALFA, Abbreviated Language for Authorization, is a domain specific language used to express XACML authorization policies. It is by far much easier to work with than writing the raw XML. Depending on who you ask it is easier to understand and work with than UI tools.

> ALFA: https://www.oasis-open.org/committees/download.php/55228/alfa-for-xacml-v1.0-wd01.doc 

```alfa
namespace hospital { 
   policyset topLevel {
      apply permitOverrides 
      medicalPolicy 
      printerPolicy
   }
   policy medicalPolicy {
      target clause Attributes.resourceType == “medical-record” 
      apply denyOverrides
      rule {
         permit
         target clause Attributes.role == “doctor”
      }
      rule { 
         deny
         condition not(booleanOneAndOnly(Attributes.careRelationExists)) 
         on deny {
            advice ObligationAdvice.reasonForDeny { 
               Attributes.message = “There is no care relation”
            } 
         }
      } 
      rule {
         deny
         condition booleanOneAndOnly(Attributes.recordIsBlocked) 
         on deny {
            advice ObligationAdvice.reasonForDeny { 
               Attributes.message = “The record is blocked”
            } 
         }
      }
   }

```


## BAL Balea Authorization Language

Instead of starting with ALFA as our default DSL, we decide to create a more simple language with a reduce number of features. BAL, Balea Authorization Language, is this simplified language to define authorization policies.

### BAL Examples

´´´BAL
policy Example begin
    rule CardiologyNurses (PERMIT) begin
        Subject.Role = ""Nurse"" 
        AND Subject.Name = ""Mary Joe""
        AND Resource.Action = ""MedicalRecord""
    end
end
´´´

´´´BAL
 policy Example begin
    rule CardiologyNurses (DENY) begin
        Subject.Age < 20 AND  Subject.Id * 1000 >= 1000 * 1
    end
end
´´´

The main important features of BAL are:

    .- Allow to write logical, aritmetic conditions, string comparison etc
    .- Allow to PERMIT or DENY rules.
    .- Allow to write multiple rules on the same policies.
    .- Allow to express target conditions.
    .- The rules can use a context to get information about the PEP, like Subject and Resource property bags.