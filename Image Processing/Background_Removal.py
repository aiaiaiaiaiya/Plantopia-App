import numpy as np
import cv2
from matplotlib import pyplot as plt

empty = cv2.imread("empty_2.jpg")
empty = cv2.resize(empty, (0,0), fx=0.1, fy=0.1)
full = cv2.imread("plant_2.jpg")
full = cv2.resize(full, (0,0), fx=0.1, fy=0.1)

# save color copy for visualization
full_c = full.copy()

# save shape (h and w) of image
px = empty.shape
h = px[0]
w = px[1]

empty_ori = empty.copy()
full_ori = full.copy()

for i in range(h):
    for j in range(w):
        if empty[i,j].all() == full[i,j].all():
            empty[i,j] = (0,0,0)
            full[i,j] = (0,0,0)
            
# convert to grayscale
empty_g = cv2.cvtColor(empty, cv2.COLOR_BGR2GRAY)
full_g = cv2.cvtColor(full, cv2.COLOR_BGR2GRAY)

# get the difference between full and empty box
#diff = full_g - empty_g
diff = cv2.absdiff(full_g, empty_g)
cv2.imwrite("diff.jpg", diff)

# inverse thresholding to change every pixel above 190 to black
_, diff_th = cv2.threshold(diff, 190, 255, 1)
cv2.imwrite("diff_1.jpg", diff_th)

# combine the difference image and the inverse threshold
crop = cv2.bitwise_and(diff, diff_th, None)
cv2.imwrite("diff_2.jpg", crop)

# threshold to get the mask instead of gray pixels
_, crop = cv2.threshold(crop, 100, 255, 0)
cv2.imwrite("diff_3.jpg", crop)

# dilate to account for the blurring in the beginning
kernel = np.ones((15, 15), np.uint8)
crop = cv2.dilate(crop, kernel, iterations=1)
_, crop = cv2.threshold(crop, 100, 255, 0)
cv2.imwrite("diff_4.jpg", crop)

# find contours, sort and draw the biggest one
_, contours, _ = cv2.findContours(crop, cv2.RETR_TREE,cv2.CHAIN_APPROX_SIMPLE)
contours = sorted(contours, key=cv2.contourArea, reverse=True)


centerPoint = [(101,190),(154,104),(149,315),(223,193)]

#draw circle in fixed position
makecircle = np.zeros((h,w), np.uint8)
for p in range(4):
    cv2.circle(makecircle,centerPoint[p],30,(255,255,255),-1)
    
#combine contour if there are in circle
_, c_contours, _ = cv2.findContours(makecircle, cv2.RETR_TREE,cv2.CHAIN_APPROX_SIMPLE)

blank_image = np.zeros((h,w), np.uint8)
for circle in c_contours:
    combine_con = []
    for C in contours:
        is_inside = False
        for point in C:
            co = (point[0][0],point[0][1])
            if cv2.pointPolygonTest(circle, co, False) > 0 or is_inside:
                is_inside = True
                combine_con.append(point)
    combine_con = np.array(combine_con)       
    cv2.drawContours(full_c, [combine_con], -1, (0, 255, 0), 3)
    (x,y),radius = cv2.minEnclosingCircle(combine_con)
    center = (int(x),int(y))
    radius = int(radius)
    cv2.circle(full_c,center,radius,(0,255,0),2)
    print ("RADIUS = ",radius)
    print ("DIAMETER = ",radius*2)

# show and save the result
plt.subplot(2,3,1),plt.imshow(empty_ori)
plt.title('Background'), plt.xticks([]), plt.yticks([])

plt.subplot(2,3,2),plt.imshow(full_ori)
plt.title('Foreground'), plt.xticks([]), plt.yticks([])

plt.subplot(2,3,3),plt.imshow(full)
plt.title('Capture cut BG'), plt.xticks([]), plt.yticks([])

plt.subplot(2,3,4),plt.imshow(diff)
plt.title('Abs Diff'), plt.xticks([]), plt.yticks([])

plt.subplot(2,3,5),plt.imshow(crop)
plt.title('Contour'), plt.xticks([]), plt.yticks([])

plt.subplot(2,3,6),plt.imshow(full_c)
plt.title('Result'), plt.xticks([]), plt.yticks([])

#Save to file
figure = plt.gcf() # get current figure
figure.set_size_inches(10, 6)
plt.savefig("plantplot_3-1.png", dpi = 100)