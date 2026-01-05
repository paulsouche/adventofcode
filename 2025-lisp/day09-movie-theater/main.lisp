

(defun read-file (filename) "Reads the file and returns a list of lines." 
  (with-open-file 
    (stream filename :direction :input) 
    (loop for line = 
      (read-line stream nil) while line collect line))) 

(defun split (char string) "Split STRING on CHAR and return a list of substrings." 
  (let 
    ((start 0) (result '())) 
  (loop for i from 0 to (length string) do 
    (when 
      (or (= i (length string)) 
        (char= (char string i) char)) 
      (push 
        (subseq string start i) result) (setf start (1+ i)))) (nreverse result))) 

(defun parse-input (raw-input) "Parse input lines into list of point plists" 
  (mapcar 
    (lambda (line) 
      (let* 
        (
          (parts (split #\, line)) 
          (x 
            (parse-integer (first parts))) 
          (y 
            (parse-integer (second parts)))) (list :x x :y y))) raw-input)) 

(defun calc-rectangle-area (point1 point2) "Calculate rectangle area from two diagonal points" 
  (* 
    (1+ 
      (abs 
        (- (getf point1 :x) (getf point2 :x)))) 
    (1+ 
      (abs 
        (- (getf point1 :y) (getf point2 :y)))))) 

(defun find-largest-rectangle (points) "Find largest rectangle area from all point pairs" 
  (let ((max-square 0)) 
    (loop for i from 0 below (length points) do 
      (loop for j from (1+ i) below (length points) do 
        (setf max-square 
          (max max-square 
            (calc-rectangle-area (nth i points) (nth j points)))))) max-square)) 

(defun to-rectangle-vertices (point1 point2) "Generate all rectangle vertices (diagonals and sides)" 
  (let 
    (
      (min-x 
        (min (getf point1 :x) (getf point2 :x))) 
      (max-x 
        (max (getf point1 :x) (getf point2 :x))) 
      (min-y 
        (min (getf point1 :y) (getf point2 :y))) 
      (max-y 
        (max (getf point1 :y) (getf point2 :y)))) 
    (list 
;; Diagonals
 (list point1 point2) 
      (list 
        (list :x (getf point1 :x) :y (getf point2 :y)) 
        (list :x (getf point2 :x) :y (getf point1 :y))) 
;; Sides
 
      (list 
        (list :x min-x :y min-y) 
        (list :x min-x :y max-y)) 
      (list 
        (list :x min-x :y min-y) 
        (list :x max-x :y min-y)) 
      (list 
        (list :x max-x :y max-y) 
        (list :x min-x :y max-y)) 
      (list 
        (list :x max-x :y max-y) 
        (list :x max-x :y min-y))))) 

(defun line-intersects (line1 line2) "Check if two line segments intersect" 
  (let* 
    (
      (x1 
        (getf (first line1) :x)) 
      (y1 
        (getf (first line1) :y)) 
      (x2 
        (getf (second line1) :x)) 
      (y2 
        (getf (second line1) :y)) 
      (x3 
        (getf (first line2) :x)) 
      (y3 
        (getf (first line2) :y)) 
      (x4 
        (getf (second line2) :x)) 
      (y4 
        (getf (second line2) :y)) 
      (det 
        (- 
          (* (- x1 x2) (- y3 y4)) 
          (* (- y1 y2) (- x3 x4))))) 
    (if (= det 0) nil 
      (let 
        (
          (lambda 
            (/ 
              (+ 
                (* (- y3 y4) (- x1 x3)) 
                (* (- x4 x3) (- y1 y3))) det)) 
          (gamma 
            (/ 
              (+ 
                (* (- y1 y2) (- x1 x3)) 
                (* (- x2 x1) (- y1 y3))) det))) 
        (and (< 0 lambda 1) (< 0 gamma 1)))))) 

(defun find-inner-largest-rectangle (points) "Find largest rectangle that doesn't intersect polygon edges" 
  (let ((segments nil)) 
    (loop for i from 1 to (length points) do 
      (push 
        (list (nth (1- i) points) 
          (nth 
            (mod i (length points)) points)) segments)) 
    (let ((max-area 0)) 
      (loop for i from 0 below (length points) do 
        (loop for j from (1+ i) below (length points) do 
          (let 
            (
              (vertices 
                (to-rectangle-vertices (nth i points) (nth j points)))) 
            (unless 
              (some 
                (lambda (vline) 
                  (some 
                    (lambda (sline) 
                      (line-intersects vline sline)) segments)) vertices) 
              (setf max-area 
                (max max-area 
                  (calc-rectangle-area (nth i points) (nth j points)))))))) max-area))) 

(defun part1 (filename) 
  (find-largest-rectangle 
    (parse-input (read-file filename)))) 

(defun part2 (filename) 
  (find-inner-largest-rectangle 
    (parse-input (read-file filename)))) 

(assert 
  (= 
    (part1 "input_test.txt") 50)) 

(print (part1 "input.txt")) 

(assert 
  (= 
    (part2 "input_test.txt") 24)) 

(print (part2 "input.txt"))
