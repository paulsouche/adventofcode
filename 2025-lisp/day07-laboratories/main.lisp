

(defun read-file (filename) "Reads the file and returns a list of lines." 
  (with-open-file 
    (stream filename :direction :input) 
    (loop for line = 
      (read-line stream nil) while line collect line))) 

(defstruct size width height) 

(defstruct parsed-result start size grid) 

(defun parse-input (lines) "Parses the input into (start size grid)." 
  (let* 
    ((start nil) 
      (height (1- (length lines))) (width 0) 
      (grid 
        (make-array (1+ height)))) 
    (loop for line in lines for y from 0 do 
      (let* 
        (
          (chars (coerce line 'list)) 
          (row 
            (make-array (length chars)))) 
        (setf (aref grid y) row) 
        (loop for ch in chars for x from 0 do 
          (setf (aref row x) (char= ch #\^)) 
          (when (char= ch #\S) 
            (setf start (list :x x :y y))) 
          (setf width (max width x))))) 
    (make-parsed-result :start start :size 
      (make-size :width width :height height) :grid grid))) 

(defun simulate-tachyon-beams (parsed) "Returns plist: (:splitters n :timelines n)" 
  (let* 
    (
      (start 
        (parsed-result-start parsed)) 
      (size 
        (parsed-result-size parsed)) 
      (grid 
        (parsed-result-grid parsed)) 
      (width (size-width size)) 
      (height (size-height size)) 
      (visited 
        (make-hash-table :test 'equal)) (splitters 0)) 
    (labels 
      (
        (dfs (x y) 
          (let* 
            (
              (key 
                (format nil "~A,~A" x y)) 
              (prev (gethash key visited))) 
            (when prev 
              (return-from dfs prev)) 
            (when 
              (or (< y 0) (> y height) (< x 0) (> x width)) 
              (setf (gethash key visited) 1) (return-from dfs 1)) 
            (let ((new-y (1+ y))) 
              (when (> new-y height) 
                (setf (gethash key visited) 1) (return-from dfs 1)) 
              (unless 
                (aref (aref grid new-y) x) 
                (setf (gethash key visited) (dfs x new-y)) 
                (return-from dfs (gethash key visited))) (incf splitters) 
              (setf (gethash key visited) 
                (+ (dfs (1+ x) new-y) (dfs (1- x) new-y))) (gethash key visited))))) 
      (let 
        (
          (timelines 
            (dfs (getf start :x) (getf start :y)))) 
        (list :splitters splitters :timelines timelines))))) 

(defun part1 (filename) 
  (let* 
    (
      (data 
        (parse-input (read-file filename))) 
      (result 
        (simulate-tachyon-beams data))) 
    (getf result :splitters))) 

(defun part2 (filename) 
  (let* 
    (
      (data 
        (parse-input (read-file filename))) 
      (result 
        (simulate-tachyon-beams data))) 
    (getf result :timelines))) 

(assert 
  (= 
    (part1 "input_test.txt") 21)) 

(print (part1 "input.txt")) 

(assert 
  (= 
    (part2 "input_test.txt") 40)) 

(print (part2 "input.txt"))
