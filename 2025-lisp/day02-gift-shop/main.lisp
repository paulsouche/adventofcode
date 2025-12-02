

(defun is-invalid-id (n) "Return T if integer n is an invalid id" 
  (let* 
    ( 
      (s (prin1-to-string n)) (len (length s))) 
    (and (evenp len) 
      (string= 
        (subseq s 0 (/ len 2)) 
        (subseq s (/ len 2) len))))) 

(assert (is-invalid-id 11)) 

(assert (is-invalid-id 22)) 

(assert (is-invalid-id 55)) 

(assert (is-invalid-id 99)) 

(assert (is-invalid-id 1010)) 

(assert (is-invalid-id 6464)) 

(assert 
  (is-invalid-id 123123)) 

(assert 
  (is-invalid-id 222222)) 

(assert 
  (is-invalid-id 446446)) 

(assert 
  (is-invalid-id 38593859)) 

(assert 
  (is-invalid-id 1188511885)) 

(assert 
  (null (is-invalid-id 111))) 

(defun has-repetition (n) "Return T if integer n to string contains a repeated substring, NIL otherwise." 
  (let* 
    ((has nil) 
      (s (prin1-to-string n)) (len (length s))) 
    (loop for sub-len from 1 to (/ len 2) when 
      (zerop (mod len sub-len)) do 
      (let 
        ( 
          (sub (subseq s 0 sub-len)) 
          (repeat-count (/ len sub-len)) (repeated "")) 
        (dotimes (i repeat-count) 
          (setf repeated 
            (concatenate 'string repeated sub))) 
        (when (string= s repeated) (setf has t) (return has)))) has)) 

(assert (has-repetition 11)) 

(assert (has-repetition 22)) 

(assert (has-repetition 99)) 

(assert (has-repetition 111)) 

(assert (has-repetition 999)) 

(assert (has-repetition 1010)) 

(assert 
  (has-repetition 222222)) 

(assert 
  (has-repetition 446446)) 

(assert 
  (has-repetition 565656)) 

(assert 
  (has-repetition 38593859)) 

(assert 
  (has-repetition 824824824)) 

(assert 
  (has-repetition 1188511885)) 

(assert 
  (has-repetition 2121212121)) 

(assert 
  (null 
    (has-repetition 123456789))) 

(defun read-input (path) "Return the first line of the file at PATH as a string, or NIL if empty." 
  (with-open-file 
    (in path :direction :input) 
    (read-line in nil nil))) 

(defun split (char string) "Split STRING on CHAR and return a list of substrings." 
  (let 
    ((start 0) (result '())) 
  (loop for i from 0 to (length string) do 
    (when 
      (or (= i (length string)) 
        (char= (char string i) char)) 
      (push 
        (subseq string start i) result) (setf start (1+ i)))) (nreverse result))) 

(defun parse-ranges (s) "Parse a comma-separated list of A-B ranges into ((A B) ...)." 
  (mapcar 
    (lambda (chunk) 
      (destructuring-bind (a b) (split #\- chunk) 
        (list (parse-integer a) (parse-integer b)))) (split #\, s))) 

(defun sum-invalid-id-in-ranges (ranges) "Sum all numbers in RANGES for which (is-invalid-id) returns true. RANGES is a list like ((a b) (c d) ...)." 
  (let ((sum 0)) 
    (dolist (range ranges sum) 
      (destructuring-bind (start end) range 
        (loop for n from start to end when (is-invalid-id n) do (incf sum n)))))) 

(defun sum-invalid-repetitions-in-ranges (ranges) "Sum all numbers in RANGES for which (has-repetition) returns true. RANGES is a list like ((a b) (c d) ...)." 
  (let ((sum 0)) 
    (dolist (range ranges sum) 
      (destructuring-bind (start end) range 
        (loop for n from start to end when (has-repetition n) do (incf sum n)))))) 

(defun part1 (path) "Sum all invalid IDs in the ranges specified in the file at PATH." 
  (sum-invalid-id-in-ranges 
    (parse-ranges (read-input path)))) 

(defun part2 (path) "Sum all IDs with repetitions in the ranges specified in the file at PATH." 
  (sum-invalid-repetitions-in-ranges 
    (parse-ranges (read-input path)))) 

(assert 
  (= 
    (part1 "input_test.txt") 1227775554)) 

(print (part1 "input.txt")) 

(assert 
  (= 
    (part2 "input_test.txt") 4174379265)) 

(print (part2 "input.txt"))
