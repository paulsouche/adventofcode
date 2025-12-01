

(defun rotate-dial 
  (currentPos instruction) "Rotate the dial from currentPos by the instruction." 
  (let 
    (
      (direction 
        (subseq instruction 0 1)) 
      (weight 
        (parse-integer 
          (subseq instruction 1)))) 
    (if 
      (string= direction "L") 
      (mod 
        (+ (- currentPos weight) 100) 100) 
      (mod (+ currentPos weight) 100) )) ) 

(assert 
  (= (rotate-dial 11 "R8") 19)) 

(assert 
  (= 
    (rotate-dial 19 "L19") 0)) 

(assert 
  (= (rotate-dial 0 "L1") 99)) 

(assert 
  (= (rotate-dial 99 "R1") 0)) 

(defun read-file (filename) "Reads the file and returns a list of lines." 
  (with-open-file 
    (stream filename :direction :input) 
    (loop for line = 
      (read-line stream nil) while line collect line))) 

(defun part1 (filename start-pos) "Count occurrences of zero position rotating the dial from start-pos by instructions in the file." 
  (let 
    (
      (lines (read-file filename)) (pos start-pos) (inc 0)) 
    (loop for line in lines do 
      (progn 
        (setf pos 
          (rotate-dial pos line)) 
        (when (zerop pos) (incf inc)))) inc)) 

(defun count-zero-occurrences 
  (currentPos instruction) "Count occurrences of zero position rotating the dial from currentPos by the instruction." 
  (let 
    (
      (direction 
        (subseq instruction 0 1)) 
      (weight 
        (parse-integer 
          (subseq instruction 1)))) 
    (if 
      (string= direction "L") 
      (if (= currentPos 0) (floor weight 100) 
        (abs 
          (floor (- currentPos weight) 100))) 
      (if 
        (= 
          (mod (+ currentPos weight) 100) 0) 
        (- 
          (floor (+ currentPos weight) 100) 1) 
        (floor (+ currentPos weight) 100)) )) ) 

(assert 
  (= 
    (count-zero-occurrences 50 "R1000") 10)) 

(defun part2 (filename start-pos) "Count all occurrences of zero position including intermediate positions." 
  (let 
    (
      (lines (read-file filename)) (pos start-pos) (inc 0)) 
    (loop for line in lines do 
      (progn 
        (setf inc 
          (+ inc 
            (count-zero-occurrences pos line))) 
        (setf pos 
          (rotate-dial pos line)) 
        (when (zerop pos) (incf inc)))) inc)) 

(assert 
  (= 
    (part1 "input_test.txt" 50) 3)) 

(print 
  (part1 "input.txt" 50)) 

(assert 
  (= 
    (part2 "input_test.txt" 50) 6)) 

(print 
  (part2 "input.txt" 50))
