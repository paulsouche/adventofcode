

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

(defun parse-input (raw-input) "Parses the raw input into a list of boxes with x, y, z coordinates and an initial circuit value of -1." 
  (loop for line in raw-input collect 
    (let* 
      (
        (parts (split #\, line)) 
        (coords 
          (mapcar #'parse-integer parts))) 
      (list :x (first coords) :y (second coords) :z (third coords) :circuit -1)))) 

(defun straight-line-distance (box1 box2) "Calculates the straight-line (Euclidean) distance between two boxes." 
  (+ 
    (expt 
      (abs 
        (- (getf box1 :x) (getf box2 :x))) 2) 
    (expt 
      (abs 
        (- (getf box1 :y) (getf box2 :y))) 2) 
    (expt 
      (abs 
        (- (getf box1 :z) (getf box2 :z))) 2))) 

(defun available-junctions (boxes) "Generates all possible junctions between boxes, sorted by distance." 
  (let ((junctions nil)) 
    (loop for i from 0 below (length boxes) do 
      (loop for j from (1+ i) below (length boxes) do 
        (push 
          (list :box1 (nth i boxes) :box2 (nth j boxes) :distance 
            (straight-line-distance (nth i boxes) (nth j boxes))) junctions))) 
    (sort junctions #'< :key 
      (lambda (j) (getf j :distance))))) 

(defun find-box (boxes target-box) "Finds and returns the box in BOXES that matches TARGET-BOX." 
  (find target-box boxes :test #'eq)) 

(defun set-circuit (box value) "Sets the circuit value of BOX to VALUE." 
  (setf (getf box :circuit) value)) 

(defun do-the-junctions (boxes junctions) "Processes the junctions to assign circuit IDs to boxes and returns a list of circuits with their sizes." 
  (let ((next-circuit-id 0)) 
    (dolist (junction junctions) 
      (let 
        (
          (first 
            (find-box boxes (getf junction :box1))) 
          (second 
            (find-box boxes (getf junction :box2)))) 
        (cond 
          (
            (and 
              (= (getf first :circuit) -1) 
              (= 
                (getf second :circuit) -1)) 
            (set-circuit first next-circuit-id) 
            (set-circuit second next-circuit-id) 
            (incf next-circuit-id)) 
          (
            (= (getf first :circuit) -1) 
            (set-circuit first 
              (getf second :circuit))) 
          (
            (= 
              (getf second :circuit) -1) 
            (set-circuit second (getf first :circuit))) 
          (t 
            (let 
              (
                (second-circuit 
                  (getf second :circuit))) 
              (dolist (box boxes) 
                (when 
                  (= (getf box :circuit) second-circuit) 
                  (set-circuit box (getf first :circuit))))))))) 
    (let ((circuits nil)) 
      (dolist (box boxes) 
        (unless 
          (= (getf box :circuit) -1) 
          (let 
            (
              (existing 
                (find (getf box :circuit) circuits :key 
                  (lambda (c) (getf c :id))))) 
            (if existing 
              (incf (getf existing :size)) 
              (push 
                (list :id (getf box :circuit) :size 1) circuits))))) 
      (sort circuits #'> :key 
        (lambda (c) (getf c :size)))))) 

(defun merge-boxes (boxes junctions) "Merges boxes using junctions until all boxes are connected, returning the product of the x-coordinates of the last merged boxes." 
  (let 
    (
      (unique-numbers (length boxes))) 
    (dolist (junction junctions) 
      (let 
        (
          (first 
            (find-box boxes (getf junction :box1))) 
          (second 
            (find-box boxes (getf junction :box2)))) 
        (when 
          (= (getf first :circuit) -1) (set-circuit first 0) (decf unique-numbers)) 
        (when 
          (= 
            (getf second :circuit) -1) 
          (set-circuit second 0) (decf unique-numbers)) 
        (when (<= unique-numbers 0) 
          (return-from merge-boxes 
            (* (getf first :x) (getf second :x)))))))) 

(defun part1 
  (filename available-cables) 
  (let* 
    (
      (boxes 
        (parse-input (read-file filename))) 
      (junctions 
        (subseq 
          (available-junctions boxes) 0 available-cables)) 
      (circuits 
        (do-the-junctions boxes junctions))) 
    (reduce #'* 
      (mapcar 
        (lambda (c) (getf c :size)) 
        (subseq circuits 0 
          (min 3 (length circuits)))) :initial-value 1))) 

(defun part2 (filename) 
  (let* 
    (
      (boxes 
        (parse-input (read-file filename))) 
      (junctions 
        (available-junctions boxes))) 
    (merge-boxes boxes junctions))) 

(assert 
  (= 
    (part1 "input_test.txt" 10) 40)) 

(print 
  (part1 "input.txt" 1000)) 

(assert 
  (= 
    (part2 "input_test.txt") 25272)) 

(print (part2 "input.txt"))
