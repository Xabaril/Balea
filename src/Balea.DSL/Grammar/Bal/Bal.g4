/**
 * Balea Authorization Language Grammar
 */
 grammar Bal;


 /**There is only one policy, which is composed by one or more rules*/
 policy
 :
 	'policy' ID
 	(

 	)? 'begin' pol_rule+ 'end'
 ;

 /** One rule of the policy. action is PERMIT by default. Target and condition are optional*/
 pol_rule
 :
 	'rule' ID
 	(
 		'(' action_id ')'
 	)? 'begin'
 	(
 		'target' condition
 	)?
 	(
 		condition
 	)? 'end'
 ;

 /** This applies to target and condition indistinctly. This is valid if you use YAAL as an 
  * understanding tool, but it does not suffice to generate XACML code
  */
 condition
 :
 	'(' condition ')'
 	| 'not' condition
 	| condition bool_op condition
 	| condition bool_comp condition
 	| arit_val arit_comp arit_val
 	| str_val str_comp str_val
 	| bool_val
 ;

 /** Possible actions to be defined */
 action_id
 :
 	'PERMIT'
 	| 'DENY'
 ;

 /** Boolean operations */
 bool_op
 :
 	'AND'
 	| 'OR'
 ;

 /** Boolean comparators */
 bool_comp
 :
 	'='
 	| '!='
 ;

 /** Arithmetic values */
 arit_val
 :
 	'(' arit_val ')'
 	| NUM
 	| categ_attr
 	| arit_val arit_op arit_val
 ;

 /** Arithmetic operator */
 arit_op
 :
 	(
 		'+'
 		| '-'
 		| '*'
 		| '/'
 		| '%'
 	)
 ;

 /** Arithmetic comparators */
 arit_comp
 :
 	'='
 	| '!='
 	| '<'
 	| '>'
 	| '>='
 	| '<='
 ;

 /** Boolean values */
 bool_val
 :
 	'TRUE'
 	| 'FALSE'
 ;

 /** Attribute associated to a category */
 categ_attr
 :
 	ID '.' ID
 ;

 /** String value */
 str_val
 :
 	categ_attr
 	| STRING
 ;

 /** String comparator */
 str_comp
 :
 	'='
 	| '<>'
 ;

 /** Identifier */
 ID
 :
 	LETTER
 	(
 		LETTER
 		| NUM
 		| '_'
 	)*
 ;

 /** Upper or lower case letter */
 LETTER
 :
 	(
 		[a-z]
 		| [A-Z]
 	)
 ;

 /** Number (base ten) */
 NUM
 :
 	[0-9]+
 ;

 /** White Space */
 WS
 :
 	[ \t\r\n]+ -> skip
 ;

 /** String */
 STRING
 :
 	'"'
 	(
 		ESC
 		| ~[\\"]
 	)* '"'
 ;

 /** Scape characters */
 ESC
 :
 	'\\"'
 	| '\\\\'
 ;

 /** Sections to ignore */
 Comment
 :
 	'#' ~( '\r' | '\n' )* -> skip
 ;