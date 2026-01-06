

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

(defun parse-input (raw-input) "Parse input into a hash table representing the graph" 
  (let 
    (
      (graph 
        (make-hash-table :test 'equal))) 
    (dolist (line raw-input) 
      (let* 
        (
          (parts (split #\: line)) (from (first parts)) 
          (to-list 
            (split #\Space (second parts)))) 
        (setf (gethash from graph) 
          (remove-if 
            (lambda (s) (string= s "")) to-list)))) graph)) 

(defun memoize (func) "Create a memoized version of a function with explicit start-end key" 
  (let 
    (
      (cache 
        (make-hash-table :test 'equal))) 
    (lambda (start end) 
      (let 
        (
          (key 
            (concatenate 'string start "-" end))) 
        (multiple-value-bind (value found) (gethash key cache) 
          (if found value 
            (setf (gethash key cache) 
              (funcall func start end)))))))) 

(defun build-find-graph-paths (graph) "Build a memoized path-finding function for the given graph" 
  (let 
    (
      (find-paths-recursive nil)) 
    (setf find-paths-recursive 
      (memoize 
        (lambda (start end) 
          (let ((paths 0)) 
            (dolist 
              (to (gethash start graph)) 
              (cond 
                ((string= to end) (incf paths)) 
                (
                  (not (gethash to graph)) nil) 
                (t 
                  (incf paths 
                    (funcall find-paths-recursive to end))))) paths)))) find-paths-recursive)) 

(defun part1 (filename) 
  (let* 
    (
      (graph 
        (parse-input (read-file filename))) 
      (find-paths 
        (build-find-graph-paths graph))) 
    (funcall find-paths "you" "out"))) 

(defun part2 (filename) 
  (let* 
    (
      (graph 
        (parse-input (read-file filename))) 
      (find-paths 
        (build-find-graph-paths graph)) 
      (start-to-dac 
        (funcall find-paths "svr" "dac")) 
      (start-to-fft 
        (funcall find-paths "svr" "fft")) 
      (dac-to-fft 
        (funcall find-paths "dac" "fft")) 
      (fft-to-dac 
        (funcall find-paths "fft" "dac")) 
      (fft-to-out 
        (funcall find-paths "fft" "out")) 
      (dac-to-out 
        (funcall find-paths "dac" "out"))) 
    (+ 
      (* start-to-dac dac-to-fft fft-to-out) 
      (* start-to-fft fft-to-dac dac-to-out)))) 

(assert 
  (= 
    (part1 "input_test.txt") 5)) 

(print (part1 "input.txt")) 

(assert 
  (= 
    (part2 "input_test2.txt") 2)) 

(print (part2 "input.txt"))
