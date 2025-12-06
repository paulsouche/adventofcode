

(defun read-file (path) "Return the content of the file at PATH as a string, or NIL if empty." 
  (with-open-file 
    (in path :direction :input) 
    (let 
      (
        (content 
          (make-string (file-length in)))) 
      (read-sequence content in) content))) 

(defun split-lines (string) "Split lines on \n." 
  (let 
    ((result '()) (start 0) (len (length string))) 
  (loop for i from 0 to len do 
    (if 
      (or (= i len) 
        (char= (aref string i) #\Newline)) 
      (let 
        (
          (substr 
            (subseq string start i))) (push substr result) (setf start (1+ i))))) (nreverse result))) 

(defun parse-range (line) "Parse range x-y into (x,y)." 
  (let 
    (
      (pos (position #\- line))) 
    (unless pos 
      (error "Invalid range line: ~A" line)) 
    (cons 
      (parse-integer line :end pos) 
      (parse-integer line :start (1+ pos))))) 

(defun parse-input (content) "Parse the input file in ranges and ids." 
  (let* 
    (
      (lines (split-lines content)) 
      (sep 
        (position "" lines :test #'string=))) 
    (unless sep 
      (error "Input missing blank line separator.")) 
    (let 
      (
        (range-lines 
          (remove "" (subseq lines 0 sep) :test #'string=)) 
        (id-lines 
          (remove "" 
            (subseq lines (1+ sep)) :test #'string=))) 
      (list :ranges 
        (mapcar #'parse-range range-lines) :ids 
        (mapcar #'parse-integer id-lines))))) 

(defun is-fresh (id ranges) "Returns T if id is fresh. e.g. is in one range." 
  (some 
    (lambda (r) 
      (let 
        ((s (car r)) (e (cdr r))) 
        (and (>= id s) (<= id e)))) ranges)) 

(defun merge-ranges (ranges) "Merge range if they overlap. Sort by start and then update end or push new range." 
  (let* 
    (
      (sorted 
        (sort (copy-list ranges) #'< :key #'car)) (merged '())) 
  (dolist (r sorted) 
    (let 
      ((start (car r)) (end (cdr r))) 
      (if 
        (or (null merged) 
          (< (cdr (car merged)) start)) 
        (push (cons start end) merged) 
        (setf (cdr (car merged)) 
          (max (cdr (car merged)) end))))) (nreverse merged))) 

(defun part1 (path) "Count the ids that are fresh." 
  (destructuring-bind (&key ranges ids) 
    (parse-input (read-file path)) 
    (let ((count 0)) 
      (dolist (id ids count) 
        (when (is-fresh id ranges) (incf count)))))) 

(defun part2 (file) "Count the possible fresh ids." 
  (destructuring-bind (&key ranges ids) 
    (parse-input (read-file file)) 
    (reduce #'+ 
      (mapcar 
        (lambda (r) 
          (+ 1 (- (cdr r) (car r)))) (merge-ranges ranges))))) 

(assert 
  (= 
    (part1 "input_test.txt") 3)) 

(print (part1 "input.txt")) 

(assert 
  (= 
    (part2 "input_test.txt") 14)) 

(print (part2 "input.txt"))
