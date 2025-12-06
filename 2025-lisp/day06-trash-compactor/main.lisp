

(defun read-file (filename) "Reads the file and returns a list of lines." 
  (with-open-file 
    (stream filename :direction :input) 
    (loop for line = 
      (read-line stream nil) while line collect line))) 

(defun do-the-math (operator operands) "Apply operator on operands." 
  (cond 
    (
      (string= operator "+") 
      (reduce #'+ operands :initial-value 0)) 
    (
      (string= operator "*") 
      (reduce #'* operands :initial-value 1)) 
    (t 
      (error "Unknown operator ~A" operator)))) 

(defun split-words (line) "Split line on \s and keep the non empty words." 
  (let 
    ((result '()) (current "")) 
  (loop for ch across line do 
    (if (char= ch #\Space) 
      (progn 
        (when 
          (> (length current) 0) (push current result)) (setf current "")) 
      (setf current 
        (concatenate 'string current (string ch))))) 
  (when 
    (> (length current) 0) (push current result)) (nreverse result))) 

(defun parse-input1 (lines) "Parses the input for part 1." 
  (let* 
    (
      (raw-lines 
        (mapcar #'split-words lines)) 
      (operators 
        (car (last raw-lines))) 
      (raw-lines (butlast raw-lines)) 
      (width (length operators)) (operands '())) 
  (dotimes (i width) 
    (let ((column '())) 
    (dolist (line raw-lines) 
      (push 
        (parse-integer (nth i line)) column)) 
    (push (nreverse column) operands))) 

(list :operators operators :operands (nreverse operands)))) 

(defun parse-input2 (lines) "Parses the input for part 2." 
  (let* 
    (
      (last-line (car (last lines))) 
      (operators 
        (split-words last-line)) 
      (lines (butlast lines)) 
      (max-len 
        (reduce #'max 
          (mapcar #'length lines))) 
      (operands (list (list)))) 
    (dotimes (i max-len) 
      (let ((acc "")) 
        (dolist (line lines) 
          (when 
            (and (< i (length line)) 
              (not 
                (char= (aref line i) #\Space))) 
            (setf acc 
              (concatenate 'string acc 
                (string (aref line i)))))) 
        (if (> (length acc) 0) 
          (push (parse-integer acc) (car (last operands))) 
          (setf operands 
            (append operands (list (list))))))) 
    (list :operators operators :operands 
      (mapcar #'nreverse operands)))) 

(defun part (filepath parse-fn) "Computes the grand total." 
  (destructuring-bind 
    (&key operators operands) 
    (funcall parse-fn (read-file filepath)) 
    (let ((sum 0)) 
      (dotimes 
        (i (length operators) sum) 
        (incf sum 
          (do-the-math (nth i operators) (nth i operands))))))) 

(defun part1 (filepath) 
  (part filepath #'parse-input1)) 

(defun part2 (filepath) 
  (part filepath #'parse-input2)) 

(assert 
  (= 
    (part1 "input_test.txt") 4277556)) 

(print (part1 "input.txt")) 

(assert 
  (= 
    (part2 "input_test.txt") 3263827)) 

(print (part2 "input.txt"))
