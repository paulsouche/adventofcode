

(defun read-file (filename) "Reads the file and returns a list of lines." 
  (with-open-file 
    (stream filename :direction :input) 
    (loop for line = 
      (read-line stream nil) while line collect line))) 

(defun parse-rolls-of-paper (lines) "Parses the grid of rolls of paper and returns a hash table of positions with '@'." 
  (let 
    ( 
      (grid 
        (make-hash-table :test 'equal))) 
    (loop for y from 0 to (1- (length lines)) do 
      (let 
        ((line (nth y lines))) 
        (loop for x from 0 to (1- (length line)) do 
          (when 
            (char= (aref line x) #\@) 
            (setf 
              (gethash 
                (format nil "~A,~A" x y) grid) t))))) grid)) 

(defun split-coordinate (coordinate) "Splits a coordinate string 'x,y' into a list of two integers (x y)." 
  (let 
    ( 
      (comma-pos 
        (position #\, coordinate))) 
    (list 
      (parse-integer 
        (subseq coordinate 0 comma-pos)) 
      (parse-integer 
        (subseq coordinate (+ comma-pos 1)))))) 

(defparameter *directions* '((-1 -1) (-1 0) (-1 1) (0 -1) (0 1) (1 -1) (1 0) (1 1))) 

(defun is-liftable (grid x y) "Determines if the roll of paper at (x, y) is liftable based on its neighbours." 
  (let ((neighbours 0)) 
    (dolist 
      (direction *directions*) 
      (let* 
        ( 
          (dx (first direction)) 
          (dy (second direction)) 
          (key 
            (format nil "~A,~A" (+ x dx) (+ y dy)))) 
        (when (gethash key grid) (incf neighbours)))) (< neighbours 4))) 

(defun part1 (input) "Count the the liftable rolls of paper in an input" 
  (let 
    ( 
      (lines (read-file input)) (grid nil)) 
    (setf grid 
      (parse-rolls-of-paper lines)) 
    (let ((liftable-count 0)) 
      (maphash 
        (lambda (key _) 
          (destructuring-bind (x y) 
            (split-coordinate key) 
            (when 
              (is-liftable grid x y) (incf liftable-count)))) grid) liftable-count))) 

(defun part2 (input) "Count the the liftable rolls of paper in an input, removes them and again until they are all non liftable." 
  (let 
    ( 
      (grid 
        (parse-rolls-of-paper (read-file input)))) 
    (let ((liftable-count 0)) 
      (loop 
        (let 
          ( 
            (round-liftable-count 0)) 
          (maphash 
            (lambda (key _) 
              (destructuring-bind (x y) 
                (split-coordinate key) 
                (when 
                  (is-liftable grid x y) (incf liftable-count) 
                  (incf round-liftable-count) (remhash key grid)))) grid) 
          (if 
            (= round-liftable-count 0) 
            (return liftable-count))))))) 

(assert 
  (= 
    (part1 "input_test.txt") 13)) 

(print (part1 "input.txt")) 

(assert 
  (= 
    (part2 "input_test.txt") 43)) 

(print (part2 "input.txt"))
