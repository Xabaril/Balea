/**
 * Balea Authorization Language Grammar
 */
 grammar Bal;

 policy
 :
 	'policy' ID 'begin' pol_rule+ 'end'
 ;

 pol_rule
 :
 	'rule' ID
 	(
 		'(' action_id ')'
 	)? 'begin'
 	(
 		condition
 	)? 'end'
 ;

 condition
 :
 	'(' condition ')'
 	| condition bool_op condition
 	| condition bool_comp condition
 	| arit_val arit_comp arit_val
 	| str_val str_comp str_val
 ;

 action_id
 :
 	'PERMIT'
 	| 'DENY'
 ;

 bool_op
 :
 	'AND'
 	| 'OR'
 ;

 bool_comp
 :
 	'='
 	| '!='
 ;

 arit_val
 :
 	'(' arit_val ')'
 	| NUM
 	| categ_attr
 	| arit_val arit_op arit_val
 ;

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

 arit_comp
 :
 	'='
 	| '!='
 	| '<'
 	| '>'
 	| '>='
 	| '<='
 ;

 categ_attr
 :
 	ID '.' ID
 ;

 str_val
 :
 	categ_attr
 	| STRING
 ;

 str_comp
 :
 	'='
 	| '<>'
 ;

 ID
 :
 	LETTER
 	(
 		LETTER
 		| NUM
 		| '_'
 	)*
 ;

 LETTER
 :
 	(
 		[a-z]
 		| [A-Z]
 	)
 ;

 NUM
 :
 	[0-9]+
 ;

 WS
 :
 	[ \t\r\n]+ -> skip
 ;

 STRING
 :
 	'"'
 	(
 		ESC
 		| ~[\\"]
 	)* '"'
 ;

 ESC
 :
 	'\\"'
 	| '\\\\'
 ;

 Comment
 :
 	'#' ~( '\r' | '\n' )* -> skip
 ;