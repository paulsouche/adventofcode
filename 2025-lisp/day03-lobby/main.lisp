

(defun read-file (filename) "Reads the file and returns a list of lines." 
  (with-open-file 
    (stream filename :direction :input) 
    (loop for line = 
      (read-line stream nil) while line collect line))) 

(defun largest-digit-combination (num-str length) "Returns the largest combination of `length` digits from num-str without changing their order." 
  (let ((result "")) 
    (loop while 
      (< (length result) length) do 
      (let 
        ((max-digit -1) (max-index -1)) 
        (loop for i from 0 to 
          (- (length num-str) 
            (- length (length result))) do 
          (let 
            ( 
              (digit 
                (digit-char-p (char num-str i)))) 
            (when (> digit max-digit) 
              (setf max-digit digit) (setf max-index i)))) 
        (setf result 
          (concatenate 'string result 
            (write-to-string max-digit))) 
        (setf num-str 
          (subseq num-str (+ max-index 1))))) 
    (parse-integer result))) 

(assert 
  (= 
    (largest-digit-combination "987654321111111" 2) 98)) 

(assert 
  (= 
    (largest-digit-combination "811111111111119" 2) 89)) 

(assert 
  (= 
    (largest-digit-combination "234234234234278" 2) 78)) 

(assert 
  (= 
    (largest-digit-combination "818181911112111" 2) 92)) 

(assert 
  (= 
    (largest-digit-combination "987654321111111" 12) 987654321111)) 

(assert 
  (= 
    (largest-digit-combination "811111111111119" 12) 811111111119)) 

(assert 
  (= 
    (largest-digit-combination "234234234234278" 12) 434234234278)) 

(assert 
  (= 
    (largest-digit-combination "818181911112111" 12) 888911112111)) 

(defun part1 (filename) "Count the sum of largest digit pairs from each line in the file." 
  (let 
    ( 
      (lines (read-file filename)) (inc 0)) 
    (loop for line in lines do 
      (setf inc 
        (+ inc 
          (largest-digit-combination line 2)))) inc)) 

(defun part2 (filename) "Count the sum of largest digit 12-combinations from each line in the file." 
  (let 
    ( 
      (lines (read-file filename)) (inc 0)) 
    (loop for line in lines do 
      (setf inc 
        (+ inc 
          (largest-digit-combination line 12)))) inc)) 

(assert 
  (= 
    (part1 "input_test.txt") 357)) 

(print (part1 "input.txt")) 

(assert 
  (= 
    (part2 "input_test.txt") 3121910778619)) 

(print (part2 "input.txt"))
