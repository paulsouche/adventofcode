

(defun read-file (filename) "Reads the file and returns a list of lines." 
  (with-open-file 
    (stream filename :direction :input) 
    (loop for line = 
      (read-line stream nil) while line collect line))) 

(defun split-string (string separator) "Split string by separator character" 
  (let 
    ((result nil) (current "")) 
    (loop for char across string do 
      (if 
        (char= char separator) 
        (progn 
          (when 
            (> (length current) 0) (push current result)) (setf current "")) 
        (setf current 
          (concatenate 'string current (string char))))) 
    (when 
      (> (length current) 0) (push current result)) (nreverse result))) 

(defun parse-line (line) "Parse a single line into expected, buttons, and joltage. Format: [.#.#] (1,2) (3,4) {5,6}" 
  (let 
    (
      (bracket-start (position #\[ line)) 
      (bracket-end (position #\] line)) 
      (brace-start (position #\{ line)) 
      (brace-end (position #\} line))) 
    (unless 
      (and bracket-start bracket-end brace-start brace-end) 
      (error "Invalid line format: ~a" line)) 
    (let* 
      (
        (expected 
          (subseq line (1+ bracket-start) bracket-end)) 
        (middle 
          (string-trim '(#\Space #\Tab #\Newline #\Return) 
          (subseq line (1+ bracket-end) brace-start))) 
      (joltage-str 
        (subseq line (1+ brace-start) brace-end)) 
      (buttons 
        (parse-buttons middle)) 
      (joltage 
        (mapcar #'parse-integer 
          (split-string joltage-str #\,)))) 
    (list :expected expected :buttons buttons :joltage joltage)))) 

(defun parse-buttons (middle) "Parse button section like '(1,2) (3,4)'" 
  (let 
    ((buttons nil) (current "") (in-parens nil)) 
    (loop for char across middle do 
      (cond 
        (
          (char= char #\() (setf in-parens t) (setf current "")) ((char= char #\)) (setf in-parens nil) 
          (when 
            (> (length current) 0) 
            (push 
              (mapcar #'parse-integer 
                (split-string current #\,)) buttons)) (setf current "")) 
        (
          (and in-parens 
            (not (char= char #\Space))) 
          (setf current 
            (concatenate 'string current (string char)))))) (nreverse buttons))) 

(defun parse-input (raw-input) "Parse input lines into list of machine indicator plists" 
  (mapcar #'parse-line raw-input)) 

(defun combinations (arr) "Generate all combinations of array elements" 
  (let 
    ((length (length arr)) (results nil)) 
    (labels 
      (
        (generate-combination 
          (start size combination) 
          (if 
            (= (length combination) size) 
            (push 
              (copy-list combination) results) 
            (loop for i from start below length do 
              (push (nth i arr) combination) 
              (generate-combination (1+ i) size combination) (pop combination))))) 
      (loop for size from 1 to length do 
        (generate-combination 0 size nil))) (nreverse results))) 

(defun find-fewest-press (machine-indicator) "Find fewest button presses needed" 
  (let* 
    (
      (expected 
        (getf machine-indicator :expected)) 
      (buttons 
        (getf machine-indicator :buttons)) 
      (possibles 
        (combinations buttons))) 
    (loop for possible in possibles do 
      (let 
        (
          (actual 
            (map 'list 
              (lambda (c) (declare (ignore c)) #\.) expected))) 
        (dolist (press possible) 
          (dolist (indicator press) 
            (setf 
              (nth indicator actual) 
              (if 
                (char= 
                  (nth indicator actual) #\.) #\# #\.)))) 
        (when 
          (string= 
            (coerce actual 'string) expected) 
          (return-from find-fewest-press (length possible))))) 0)) 

(defun get-button-masks (machine-indicator) "Convert buttons to bitmasks" 
  (mapcar 
    (lambda (button) 
      (let ((mask 0)) 
        (dolist (id button) 
          (setf mask 
            (logior mask (ash 1 id)))) mask)) 
    (getf machine-indicator :buttons))) 

(defun get-parity-states 
  (machine-indicator button-masks) "Generate parity states for all button combinations" 
  (let* 
    (
      (buttons 
        (getf machine-indicator :buttons)) 
      (joltage 
        (getf machine-indicator :joltage)) 
      (b-len (length buttons)) 
      (parity-states 
        (make-array 
          (ash 1 (length joltage)) :initial-element nil))) 
    (loop for mask from 0 below (ash 1 b-len) do 
      (let ((parity-state 0)) 
        (loop for i from 0 below b-len do 
          (when (logbitp i mask) 
            (setf parity-state 
              (logxor parity-state (nth i button-masks))))) 
        (push mask 
          (aref parity-states parity-state)))) parity-states)) 

(defun count-bits (n) "Count number of set bits in integer" 
  (let ((count 0)) 
    (loop while (> n 0) do 
      (incf count (logand n 1)) (setf n (ash n -1))) count)) 

(defun joltage-to-key (joltage) "Convert joltage list to string key for memoization" 
  (format nil "~{~a~^,~}" joltage)) 

(defun find-joltage-fewest-press 
  (joltage parity-states button-masks buttons memo) "Find fewest presses with joltage constraints using memoization" 
  (when 
    (every 
      (lambda (val) (= val 0)) joltage) 
    (return-from find-joltage-fewest-press 0)) 
  (let 
    (
      (key 
        (joltage-to-key joltage))) 
    (when (gethash key memo) 
      (return-from find-joltage-fewest-press (gethash key memo)))) 
  (let* 
    (
      (parity-state 
        (loop for val in joltage for i from 0 when (oddp val) sum (ash 1 i))) 
      (combinations 
        (aref parity-states parity-state))) 
    (when (null combinations) 
      (setf 
        (gethash 
          (joltage-to-key joltage) memo) most-positive-fixnum) 
      (return-from find-joltage-fewest-press most-positive-fixnum)) 
    (let 
      (
        (best most-positive-fixnum)) 
      (dolist (mask combinations) 
        (let 
          (
            (next-joltage (copy-list joltage))) 
          (loop for button in buttons for i from 0 do 
            (when (logbitp i mask) 
              (dolist (id button) 
                (decf (nth id next-joltage))))) 
          (unless 
            (or 
              (some 
                (lambda (val) (< val 0)) next-joltage) 
              (some #'oddp next-joltage)) 
            (let 
              (
                (sub-result 
                  (find-joltage-fewest-press 
                    (mapcar 
                      (lambda (val) (floor val 2)) next-joltage) parity-states button-masks buttons memo))) 
              (when 
                (< sub-result most-positive-fixnum) 
                (let 
                  (
                    (total 
                      (+ (count-bits mask) (* 2 sub-result)))) 
                  (when (< total best) (setf best total)))))))) 
      (setf 
        (gethash 
          (joltage-to-key joltage) memo) best) best))) 

(defun part1 (filename) 
  (let 
    (
      (machine-indicators 
        (parse-input (read-file filename)))) 
    (reduce 
      (lambda (acc indicator) 
        (+ acc 
          (find-fewest-press indicator))) machine-indicators :initial-value 0))) 

(defun part2 (filename) 
  (let 
    (
      (machine-indicators 
        (parse-input (read-file filename)))) 
    (reduce 
      (lambda 
        (acc machine-indicator) 
        (let* 
          (
            (button-masks 
              (get-button-masks machine-indicator)) 
            (parity-states 
              (get-parity-states machine-indicator button-masks)) 
            (memo 
              (make-hash-table :test 'equal))) 
          (+ acc 
            (find-joltage-fewest-press 
              (getf machine-indicator :joltage) parity-states button-masks 
              (getf machine-indicator :buttons) memo)))) machine-indicators :initial-value 0))) 

(assert 
  (= 
    (part1 "input_test.txt") 7)) 

(print (part1 "input.txt")) 

(assert 
  (= 
    (part2 "input_test.txt") 33)) 

(print (part2 "input.txt"))
