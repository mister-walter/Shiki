VFXCraft
=================
Thanks for your download!
For any support or questions please email me at vfxcraft.contact+Unity@gmail.com
All textures are in .psd format so they can be edited as desired


PREFABS
=================
All particle systems are located in “VFXCraft  > Prefabs”
Every particle system should work by simply dragging and dropping into the scene.


Resizing
=================
If the particles aren’t an appropriate size for your project you can follow these steps to adjust the size as needed.

1. Select the appropriate prefab
2. Select Shape in the particle system component
3. Change Box X and Box Y (Box shape) or Radius (Sphere) until the effect cover the appropriate sized area
4. Selec Emission in the particle system component
5. Change Rate until the desired number of particles is reached


RAIN
=================
The rain effect has a sub emitter to create a splatter when it collides with the ground. In order for this to work it is currently set up to work with a layer called “VFXCollision”
This can be changed if needed using the following steps.

Select the Rain prefab
Select Collision in the particle system component
Change the Collides With field to the desired layer

If desired this can be set to collide with everything but isn’t advisable for performance